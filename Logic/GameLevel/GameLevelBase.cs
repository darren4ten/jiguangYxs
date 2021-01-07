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
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.MutedCards;
using Logic.Model.Enums;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;

namespace Logic.GameLevel
{
    public abstract class GameLevelBase
    {
        /// <summary>
        /// 当前关卡全局事件管理器
        /// </summary>
        public readonly EventBus GlobalEventBus = EventBus.GetInstance();

        public int LevelId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// 游戏是否结束
        /// </summary>
        public bool IsGameOver { get; set; }

        protected Queue<CardBase> UnUsedCardStack { get; set; }
        protected Queue<CardBase> UsedCardStack { get; set; }

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
            return Players.Where(p => p.IsAlive());
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
            var dist = new PlayerDistance() { };
            var targetPlayerId = target.PlayerId;
            Player nextPlayer = src.GetNextPlayer(false);
            while (nextPlayer.PlayerId != targetPlayerId && dist.ShaDistance < 10)
            {
                dist.ShaDistanceWithouWeapon++;
                dist.TannangDistance++;
                nextPlayer = nextPlayer.GetNextPlayer(false);
            }
            //查看toPlayer是否装备防御马
            //获取探囊距离
            dist.TannangDistance =
                dist.TannangDistance + target.GetCurrentPlayerHero().BaseAttackFactor.DefenseDistance;
            dist.ShaDistance = dist.ShaDistanceWithouWeapon - src.GetCurrentPlayerHero().GetAttackFactor().ShaDistance;
            dist.ShaDistance = dist.ShaDistance <= 1 ? 1 : dist.ShaDistance;
            return dist;
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
        public virtual void ThrowCardToStack(List<CardBase> cards)
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

        public virtual void OnLoad(Player currentPlayer, List<Player> aditionalPlayers)
        {
            this.CurrentPlayer = currentPlayer;
            this.TempCardDesk = new TempCardDesk();
            this.WashCards();
            this.LoadPlayers(currentPlayer, aditionalPlayers);
            SetupPlayers();
            SetupGameStatusCheck();
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
        /// 开始游戏
        /// </summary>
        /// <param name="currentPlayer">当前玩家</param>
        /// <param name="aditionalPlayers">其他玩家</param>
        /// <returns></returns>
        public virtual async Task Start(Player currentPlayer, List<Player> aditionalPlayers)
        {
            OnLoad(currentPlayer, aditionalPlayers);
            Console.WriteLine("Game Started!");
            var curPlayer = GetXianshouPlayer();
            while (!IsGameOver)
            {
                await curPlayer.StartMyRound();
                curPlayer = curPlayer.GetNextPlayer(false);
            }

            IsGameOver = true;
            Console.WriteLine("Game End!");
        }

        public virtual void End()
        {
            //释放所有对象,players,skills,eventbus

            this.Players = null;
            this.UnUsedCardStack = null;
            this.UsedCardStack = null;
            //Play animation
            Console.WriteLine("Game over!");
        }

        /// <summary>
        /// 设置游戏状态检查
        /// </summary>
        public virtual void SetupGameStatusCheck()
        {
            HostPlayerHero = new PlayerHero(1, new Xiangyu(), null, null);
            //监控玩家死亡事件。
            //如果有玩家死亡，则判断是否该方队友是否全部阵亡，如果是，则代表该方玩家游戏结束
            //目前简单处理，我方（当前Player）死亡或者对方全部死亡，则游戏结束.todo:各方是否游戏结束要看具体的条件
            this.GlobalEventBus.ListenEvent(Guid.NewGuid(), HostPlayerHero, EventTypeEnum.AfterDying, (
                async (context, roundContext, responseContext) =>
                {
                    //我方死亡，则游戏结束
                    if (CurrentPlayer == context.SrcPlayer)
                    {
                        IsGameOver = true;
                        await NotifyPlayersGameEnd(CurrentPlayer.GroupId);
                        return;
                    }
                    //对方全部死亡，则游戏结束
                    if (context.SrcPlayer != null && Players.All(p => !p.IsAlive() && p.GroupId == context.SrcPlayer.GroupId))
                    {
                        IsGameOver = true;
                        await NotifyPlayersGameEnd(context.SrcPlayer.GroupId);
                        return;
                    }
                    await Task.FromResult(0);
                }));
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
                //有人响应出牌，则取其中一个才真实出牌
                var extraRes = await responseCard.PlayCard(new CardRequestContext(), null);
                if (response.ResponseResult == ResponseResultEnum.Wuxiekeji && extraRes.ResponseResult == ResponseResultEnum.Wuxiekeji)
                {
                    response.ResponseResult = ResponseResultEnum.Failed;
                    response.Message = "";
                }
            }

            return response ?? new CardResponseContext();
        }

        /// <summary>
        /// 通知玩家游戏结束
        /// </summary>
        /// <param name="victorGroupId">胜利方的groupdId</param>
        /// <returns></returns>
        protected virtual async Task NotifyPlayersGameEnd(Guid victorGroupId)
        {
            foreach (var p in Players)
            {
                if (p.GroupId == victorGroupId)
                {
                    await NotifyPlayerGameEnd(p, true);
                }
                else
                {
                    await NotifyPlayerGameEnd(p, false);
                }
            }
        }

        /// <summary>
        /// 通知玩家游戏结束
        /// </summary>
        /// <param name="player"></param>
        /// <param name="isSucess"></param>
        /// <returns></returns>
        protected virtual async Task NotifyPlayerGameEnd(Player player, bool isSucess)
        {
            await Task.FromResult(0);
        }

        private void ThrowBaseCardsToStack(List<CardBase> cards)
        {
            foreach (var cardBase in cards)
            {
                //先清除掉上下文
                cardBase.AttachPlayerContext(null);
                UnUsedCardStack.Enqueue(cardBase);
            }
        }
    }
}
