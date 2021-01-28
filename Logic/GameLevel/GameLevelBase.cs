using Logic.Cards;
using Logic.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.Event;
using Logic.Log;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Cards.MutedCards;
using Logic.Model.Enums;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using Logic.Model.RequestResponse.Request;
using Logic.Util.Extension;
using System.ComponentModel;
using Logic.Annotations;
using System.Runtime.CompilerServices;

namespace Logic.GameLevel
{
    public abstract class GameLevelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GameLevelBase()
        {
            LogManager = new FlowDocLogMananger();
        }

        /// <summary>
        /// 当前关卡全局事件管理器
        /// </summary>
        public readonly EventBus GlobalEventBus = EventBus.GetInstance();

        public readonly LogMangerBase LogManager;

        public int LevelId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// 游戏是否结束
        /// </summary>
        public bool IsGameOver { get; set; }

        /// <summary>
        /// 胜利方组Id
        /// </summary>
        public Guid VictorGroupId { get; set; }

        public Queue<CardBase> UnUsedCardStack { get; set; }
        protected Queue<CardBase> UsedCardStack { get; set; }

        /// <summary>
        /// 牌堆中剩余卡牌数
        /// </summary>
        public int RemainCardCount => UnUsedCardStack.Count;


        public List<Player> Players { get; private set; }

        /// <summary>
        /// 游戏主持人
        /// </summary>
        public PlayerHero HostPlayerHero { get; private set; }

        /// <summary>
        /// 当前的Player
        /// </summary>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        /// todo:用来存放回合所出的牌,回合结束的时候放置入弃牌堆
        /// </summary>
        public TempCardDesk TempCardDesk { get; set; }

        protected List<CardBase> CardsOnDesk { get; set; }

        /// <summary>
        /// 重新洗牌
        /// </summary>
        protected virtual void WashCards()
        {
            var unUsedCardList = new List<CardBase>();
            var util = new CardStackUtil();
            //todo:触发洗牌动画
            if (UnUsedCardStack != null && UnUsedCardStack.Any())
            {
                unUsedCardList.AddRange(UnUsedCardStack);
                unUsedCardList.AddRange(util.GenerateNewCardStack(UsedCardStack.ToList()));
            }
            else
            {
                unUsedCardList = util.GenerateNewCardStack().ToList();
            }
            this.UnUsedCardStack = new Queue<CardBase>(unUsedCardList);
            this.UsedCardStack = new Queue<CardBase>();
            OnPropertyChanged("RemainCardCount");
        }

        protected virtual void LoadPlayers(Player currentPlayer, List<Player> aditionalPlayers)
        {
            this.Players = new List<Player>();
            Players.Add(currentPlayer);
            Players.AddRange(aditionalPlayers);
        }

        /// <summary>
        /// 获取所有活着的玩家
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Player> GetAlivePlayers()
        {
            return Players.Where(p => p.IsAlive);
        }

        /// <summary>
        /// 摸牌
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual IEnumerable<CardBase> PickNextCardsFromStack(int count)
        {
            //如果取牌数>=当前剩余牌数，则重新洗牌之后再取牌
            if (count >= UnUsedCardStack.Count)
            {
                WashCards();
            }
            for (int i = 0; i < count; i++)
            {
                yield return UnUsedCardStack.Dequeue();
            }
            OnPropertyChanged("RemainCardCount");
        }

        /// <summary>
        /// 执行判定
        /// </summary>
        /// <param name="request"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual async Task<CardResponseContext> Panding(CardRequestContext request, Expression<Func<CardBase, bool>> expression)
        {
            var response = new CardResponseContext() { Cards = new List<CardBase>() };
            await GlobalEventBus.TriggerEvent(EventTypeEnum.BeforePanding, HostPlayerHero, request, null, response);
            await GlobalEventBus.TriggerEvent(EventTypeEnum.Panding, HostPlayerHero, request, null, response);
            if (response.ResponseResult == ResponseResultEnum.Cancelled)
            {
                return response;
            }
            var pdCard = PickNextCardsFromStack(1).FirstOrDefault();
            response.Cards.Add(pdCard);
            var isSuccess = expression.Compile().Invoke(pdCard);
            if (isSuccess)
            {
                response.ResponseResult = ResponseResultEnum.Success;
            }
            else
            {
                response.ResponseResult = ResponseResultEnum.Failed;
            }
            //将判定牌置入弃牌堆
            TempCardDesk.Add(pdCard);
            await GlobalEventBus.TriggerEvent(EventTypeEnum.AfterPanding, HostPlayerHero, request, null, response);
            return response;
        }

        /// <summary>
        /// 获取两个player之间的距离
        /// </summary>
        /// <param name="src"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public PlayerDistance GetPlayersDistance(Player src, Player target)
        {
            //获取物理距离
            var dist1 = GetPlayersDistanceOnReverseTimeClock(src, target);
            var dist2 = GetPlayersDistanceOnReverseTimeClock(target, src);
            var actDist = Math.Min(dist1, dist2);
            var dist = new PlayerDistance();
            dist.ShaDistance = actDist;
            dist.TannangDistance = actDist;
            dist.ShaDistanceWithoutWeapon = actDist;

            dist.TannangDistance =
                dist.TannangDistance + target.CurrentPlayerHero.BaseAttackFactor.DefenseDistance - src.CurrentPlayerHero.BaseAttackFactor.TannangDistance;
            dist.ShaDistanceWithoutWeapon += target.CurrentPlayerHero.BaseAttackFactor.DefenseDistance - src.CurrentPlayerHero.BaseAttackFactor.TannangDistance;
            dist.ShaDistance = dist.ShaDistanceWithoutWeapon - src.CurrentPlayerHero.GetAttackFactor().ShaDistance + 1;
            return dist;
        }

        public int GetPlayersDistanceOnReverseTimeClock(Player src, Player target)
        {
            var shaDistance = 1;
            var targetPlayerId = target.PlayerId;
            //正向距离
            Player nextPlayer = src.GetNextPlayer(false);
            while (nextPlayer.PlayerId != targetPlayerId && shaDistance < 10)
            {
                shaDistance++;
                nextPlayer = nextPlayer.GetNextPlayer(false);
            }

            return shaDistance;
        }


        /// <summary>
        /// 获取所有可达的敌人
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        /// <param name="attackType"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Player>> GetAvaliableEnermies(Player player, CardBase card, AttackTypeEnum attackType)
        {
            var players = new List<Player>();
            foreach (var enemy in GetAlivePlayers().Where(p => !p.IsSameGroup(player)))
            {
                if (await enemy.IsAvailableForPlayer(player, card, attackType))
                {
                    players.Add(enemy);
                }
            }

            return players;
        }

        /// <summary>
        /// 设置Player间的关联关系
        /// </summary>
        protected void SetupPlayers()
        {
            for (int index = 0; index < Players.Count; index++)
            {
                if (index == 0)
                {
                    Players[index].SetPreviousPlayer(Players[Players.Count - 1]);
                }
                else
                {
                    Players[index].SetPreviousPlayer(Players[index - 1]);
                }

                if (index == Players.Count - 1)
                {
                    Players[index].SetNextPlayer(Players[0]);
                }
                else
                {
                    Players[index].SetNextPlayer(Players[index + 1]);
                }
            }
        }

        /// <summary>
        /// 将牌置于弃牌堆
        /// </summary>
        /// <param name="cards"></param>
        public virtual void ThrowCardToStack(IEnumerable<CardBase> cards)
        {
            foreach (var card in cards)
            {
                if (card is ChangedCard)
                {
                    ThrowBaseCardsToStack((card as ChangedCard).OriginalCards);
                }
                else
                {
                    ThrowBaseCardsToStack(new List<CardBase>() { card });
                }
            }
        }

        /// <summary>
        /// 游戏开始时玩家手中的牌。如果初始摸牌数有变化，可以重写这个方法。
        /// </summary>
        public virtual void InitCardsForPlayers(Player currentPlayer, List<Player> aditionalPlayers)
        {
            var cards1 = PickNextCardsFromStack(4).ToList();
            currentPlayer.CardsInHand.AddRange(cards1.Select(c => c.AttachPlayerContext(new PlayerContext()
            {
                Player = currentPlayer,
                GameLevel = this
            })));
            foreach (var aditionalPlayer in aditionalPlayers)
            {
                aditionalPlayer.CardsInHand.AddRange(PickNextCardsFromStack(4).ToList().Select(c => c.AttachPlayerContext(new PlayerContext()
                {
                    Player = aditionalPlayer,
                    GameLevel = this
                })));
            }
        }


        public virtual void OnLoad(Player currentPlayer, List<Player> aditionalPlayers)
        {
            this.CurrentPlayer = currentPlayer;
            this.TempCardDesk = new TempCardDesk();
            this.WashCards();
            this.LoadPlayers(currentPlayer, aditionalPlayers);
            SetupPlayers();
            SetupGameStatusCheck();
        }

        public virtual async Task OnAfterLoaded()
        {
            await Task.FromResult(0);
        }

        /// <summary>
        /// 获取最先出牌的player
        /// </summary>
        /// <returns></returns>
        protected virtual Player GetXianshouPlayer()
        {
            var maxXianshou = Players.Max(p => p.GetXianshou());
            var curPlayer = Players.FirstOrDefault(p => p.GetXianshou() == maxXianshou);
            return curPlayer;
        }

        /// <summary>
        /// 开始游戏。在此方法中构造初始化玩家
        /// </summary>
        /// <returns></returns>
        public abstract Task Start(Action action = null);

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="currentPlayer">当前玩家</param>
        /// <param name="aditionalPlayers">其他玩家</param>
        /// <param name="action">OnAfterLoaded之后需要执行的代码</param>
        /// <returns></returns>
        public virtual async Task Start(Player currentPlayer, List<Player> aditionalPlayers, Action action = null)
        {
            OnLoad(currentPlayer, aditionalPlayers);
            InitCardsForPlayers(currentPlayer, aditionalPlayers);
            var curPlayer = GetXianshouPlayer();
            await OnAfterLoaded();
            action?.Invoke();
            Console.WriteLine("Game Started!");
            LogManager.LogAction(new RichTextParagraph(new RichTextWrapper("游戏开始！", RichTextWrapper.GetColor(ColorEnum.Blue))));
            while (!IsGameOver)
            {
                await curPlayer.StartMyRound();
                curPlayer = curPlayer.GetNextPlayer(false);
            }

            IsGameOver = true;
            Console.WriteLine("Game End!");
            End();
        }

        public virtual void End()
        {
            //释放所有对象,players,skills,eventbus

            this.Players = null;
            this.UnUsedCardStack = null;
            this.UsedCardStack = null;
            //Play animation
            Console.WriteLine("Game over!");
            LogManager.LogAction(new RichTextParagraph(new RichTextWrapper("游戏结束！", RichTextWrapper.GetColor(ColorEnum.Blue))));
        }

        /// <summary>
        /// 设置游戏状态检查
        /// </summary>
        public virtual void SetupGameStatusCheck()
        {
            HostPlayerHero = new PlayerHero(1, new Xiangyu(), null, null);

            //处理掉血
            EventBus.RoundEventHandler roundHandler = async (context, roundContext, responseContext) =>
            {
                if (context.AdditionalContext is Player srcPlayer)
                {
                    if (srcPlayer.CurrentPlayerHero.CurrentLife <= 0)
                    {
                        await this.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeDying, HostPlayerHero, context, roundContext, responseContext);
                    }
                }
            };
            this.GlobalEventBus.ListenEvent(Guid.NewGuid(), HostPlayerHero, EventTypeEnum.AfterLoseLife, (roundHandler));
            this.GlobalEventBus.ListenEvent(Guid.NewGuid(), HostPlayerHero, EventTypeEnum.AfterAddLife, (roundHandler));

            //监控玩家死亡事件。
            //如果有玩家死亡，则判断是否该方队友是否全部阵亡，如果是，则代表该方玩家游戏结束
            //目前简单处理，我方（当前Player）死亡或者对方全部死亡，则游戏结束.todo:各方是否游戏结束要看具体的条件
            EventBus.RoundEventHandler roundDyingHandler = async (context, roundContext, responseContext) =>
            {
                if (context.AdditionalContext is Player srcPlayer)
                {
                    //死前求药:todo:yao.PlayeCard()
                    var res = await GroupRequestWithConfirm(new CardRequestContext()
                    {
                        RequestCard = new Yao(),
                        SrcPlayer = srcPlayer,
                        MinCardCountToPlay = 1,
                        MaxCardCountToPlay = 1,
                        AttackType = AttackTypeEnum.Qiuyao,
                        TargetPlayers = Players.Where(p => p.IsAlive).ToList()
                    });

                    if (res.ResponseResult != ResponseResultEnum.Success)
                    {
                        //没有人出药则死亡
                        await srcPlayer.MakeDie();
                        //如果玩家死亡，则判断游戏是否结束
                        if (!srcPlayer.IsAlive)
                        {
                            //通知所有人该玩家死亡
                            await NotifyPlayerDeath(srcPlayer);

                            //检查该阵营是否都阵亡了，如果是，则通知该阵营游戏失败
                            if (Players.Where(p => p.GroupId == srcPlayer.GroupId).All(p => !p.IsAlive))
                            {
                                foreach (var player in Players.Where(p => p.GroupId == srcPlayer.GroupId))
                                {
                                    await NotifyPlayerFailure(player);
                                }
                            }

                            //检查当前是否只剩下一个阵营，如果是，则游戏结束，该阵营玩家胜利
                            if (GetAlivePlayers().Select(p => p.GroupId).Distinct().Count() == 1)
                            {
                                var first = GetAlivePlayers().First();
                                foreach (var player in Players.Where(p => p.GroupId == first.GroupId))
                                {
                                    await NotifyPlayerSuccess(player);
                                }

                                VictorGroupId = first.GroupId;
                                IsGameOver = true;
                            }
                        }
                    }
                }
            };
            this.GlobalEventBus.ListenEvent(Guid.NewGuid(), HostPlayerHero, EventTypeEnum.BeforeDying, (roundDyingHandler));

        }
        private object TmpResponseLock = new object();
        /// <summary>
        /// 并发请求出牌，只需要某一个人出牌即可。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CardResponseContext> GroupRequestWithConfirm(CardRequestContext request)
        {
            if (request?.TargetPlayers == null || !request.TargetPlayers.Any())
            {
                throw new Exception("没有请求目标");
            }

            if (request.SrcPlayer == null)
            {
                throw new Exception("没有来源目标");
            }

            var tasks = new List<Task<CardResponseContext>>();
            foreach (var requestTargetPlayer in request.TargetPlayers)
            {
                tasks.Add(requestTargetPlayer.ActionManager.OnParallelRequestResponseCard(new CardRequestContext()
                {
                    AttackType = request.AttackType,
                    RequestCard = request.RequestCard,
                    SrcPlayer = request.SrcPlayer,
                    MaxCardCountToPlay = request.MaxCardCountToPlay,
                    MinCardCountToPlay = request.MinCardCountToPlay,
                    AttackDynamicFactor = request.AttackDynamicFactor,
                    TargetPlayers = new List<Player>()
                    {
                        requestTargetPlayer
                    }
                }));
            }

            CardResponseContext response = null;
            do
            {
                var task = await Task.WhenAny<CardResponseContext>(tasks);
                var tmpResult = await task;
                if (tmpResult.Cards?.Any() == true)
                {
                    response = tmpResult;
                }
                tasks.Remove(task);
            } while (response == null && tasks.Count > 0);

            var responseCard = response?.Cards?.FirstOrDefault();
            if (responseCard != null)
            {
                response.ResponseResult = request.RequestCard is Wuxiekeji ? ResponseResultEnum.Wuxiekeji : ResponseResultEnum.Success;
                //有人响应出牌，则取其中一个才真实出牌
                var extraRes = await responseCard.PlayCard(new CardRequestContext()
                {
                    TargetPlayers = new List<Player>()
                    {
                        responseCard.PlayerContext.Player
                    }
                }, null);
                if (response.ResponseResult == ResponseResultEnum.Wuxiekeji && extraRes.ResponseResult == ResponseResultEnum.Wuxiekeji)
                {
                    response.ResponseResult = ResponseResultEnum.Failed;
                    response.Message = "";
                }
            }

            return response ?? new CardResponseContext();
        }

        /// <summary>
        /// 通知玩家游戏成功
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        protected virtual async Task NotifyPlayerSuccess(Player player)
        {
            Console.WriteLine($"***{player.PlayerId}【{player.CurrentPlayerHero.Hero.DisplayName}】玩家您游戏胜利。");
            LogManager.LogAction(new RichTextParagraph(
                new RichTextWrapper("***"),
                new RichTextWrapper($"{player.PlayerId}【{player.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue)),
                new RichTextWrapper("玩家游戏", RichTextWrapper.GetColor(ColorEnum.Blue)),
                new RichTextWrapper("胜利", RichTextWrapper.GetColor(ColorEnum.Red)),
                new RichTextWrapper("!", RichTextWrapper.GetColor(ColorEnum.Black))
                ));
            await Task.FromResult(0);
        }

        /// <summary>
        /// 通知玩家游戏失败
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        protected virtual async Task NotifyPlayerFailure(Player player)
        {
            //通知玩家本人死亡
            Console.WriteLine($"***{player.PlayerId}【{player.CurrentPlayerHero.Hero.DisplayName}】玩家您游戏失败。");
            LogManager.LogAction(new RichTextParagraph(
                  new RichTextWrapper("***"),
                  new RichTextWrapper($"{player.PlayerId}【{player.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue)),
                  new RichTextWrapper("玩家游戏", RichTextWrapper.GetColor(ColorEnum.Blue)),
                  new RichTextWrapper("失败", RichTextWrapper.GetColor(ColorEnum.Red)),
                new RichTextWrapper("!", RichTextWrapper.GetColor(ColorEnum.Black))
                  ));
            await Task.FromResult(0);
        }

        /// <summary>
        /// 通知玩家死亡
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        protected virtual async Task NotifyPlayerDeath(Player player)
        {
            //通知玩家本人死亡
            Console.WriteLine($"***{player.PlayerId}【{player.CurrentPlayerHero.Hero.DisplayName}】玩家您已经死亡。");
            LogManager.LogAction(new RichTextParagraph(
                 new RichTextWrapper("***"),
                 new RichTextWrapper($"{player.PlayerId}【{player.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue)),
                 new RichTextWrapper("玩家您已经", RichTextWrapper.GetColor(ColorEnum.Blue)),
                 new RichTextWrapper("死亡", RichTextWrapper.GetColor(ColorEnum.Red)),
               new RichTextWrapper("!", RichTextWrapper.GetColor(ColorEnum.Black))
                 ));
            //通知在场所有的人player死亡
            await Task.FromResult(0);
        }

        /// <summary>
        /// 通知玩家游戏结束
        /// </summary>
        /// <param name="player"></param>
        /// <param name="isVictor">玩家是否胜利</param>
        /// <returns></returns>
        protected virtual async Task NotifyPlayerGameEnd(Player player, bool isVictor)
        {
            Console.WriteLine($"***{player.PlayerId}【{player.CurrentPlayerHero.Hero.DisplayName}】玩家您已经{(isVictor ? "胜利！" : "失败！")}");
            LogManager.LogAction(new RichTextParagraph(
                  new RichTextWrapper("***"),
                  new RichTextWrapper($"{player.PlayerId}【{player.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue)),
                  new RichTextWrapper("玩家您已经", RichTextWrapper.GetColor(ColorEnum.Blue)),
                  new RichTextWrapper((isVictor ? "胜利" : "失败"), RichTextWrapper.GetColor(ColorEnum.Red)),
                new RichTextWrapper("!", RichTextWrapper.GetColor(ColorEnum.Black))
                  ));
            await Task.FromResult(0);
        }

        private void ThrowBaseCardsToStack(List<CardBase> cards)
        {
            foreach (var cardBase in cards)
            {
                //先清除掉上下文
                cardBase.AttachPlayerContext(null);
                UsedCardStack.Enqueue(cardBase);
            }
            OnPropertyChanged("RemainCardCount");
        }
    }
}
