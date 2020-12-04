using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Cards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Interface;
using Logic.Model.Mark;
using Logic.Util;

namespace Logic.Model.Player
{
    /// <summary>
    /// 玩家，可以包含多个英雄
    /// </summary>
    public class Player
    {
        #region 属性
        public int PlayerId { get; set; }

        /// <summary>
        /// 玩家姓名
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// 玩家描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 进入角色回合的次数
        /// </summary>
        public int Round { get; set; }

        public PlayerUIState PlayerUiState { get; }

        #region 牌
        /// <summary>
        /// 手牌
        /// </summary>
        public List<CardBase> CardsInHand { get; set; }

        /// <summary>
        /// 玩家当前的装备牌
        /// </summary>
        public List<EquipmentBase> EquipmentSet { get; set; }

        #endregion

        /// <summary>
        /// 标记(牌)，如画地为牢、手捧雷
        /// </summary>
        public List<Mark.MarkBase> Marks { get; set; }

        /// <summary>
        /// 可选的用户英雄
        /// </summary>
        protected List<PlayerHero> _availablePlayerHeroes { get; set; }

        /// <summary>
        /// 当前关卡
        /// </summary>
        protected GameLevelBase _gameLevel;

        /// <summary>
        /// 回合上下文
        /// </summary>
        public RoundContext RoundContext { get; set; }

        /// <summary>
        /// 请求出牌的上下文
        /// </summary>
        public CardRequestContext CardRequestContext { get; set; }

        /// <summary>
        /// 前一个玩家
        /// </summary>
        public Player PreviousPlayer { get; private set; }

        /// <summary>
        /// 后一个玩家
        /// </summary>
        public Player NextPlayer { get; private set; }

        #endregion

        public Player(GameLevelBase gameLevel, List<PlayerHero> availablePlayerHeroes)
        {
            _availablePlayerHeroes = availablePlayerHeroes;
            _availablePlayerHeroes.ForEach(p =>
            {
                p.SetPlayerContext(new PlayerContext()
                {
                    GameLevel = _gameLevel,
                    Player = this
                });
            });
            _gameLevel = gameLevel;
            CardsInHand = new List<CardBase>();
            Marks = new List<Mark.MarkBase>();
            EquipmentSet = new List<EquipmentBase>();
            PlayerUiState = new PlayerUIState(this);
        }

        /// <summary>
        /// 当前用户英雄
        /// </summary>
        public PlayerHero GetCurrentPlayerHero()
        {
            return _availablePlayerHeroes.OrderBy(p => p.IsDead).First();
        }

        /// <summary>
        /// 主动出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        public async Task<CardResponseContext> PlayCard(CardRequestContext cardRequestContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 响应出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        public async Task<CardResponseContext> ResponseCard(CardRequestContext cardRequestContext)
        {
            var res = await GetCurrentPlayerHero().ActionManager.OnRequestResponseCard(cardRequestContext);
            return res;
        }

        /// <summary>
        /// 开始进入我的回合
        /// </summary>
        /// <returns></returns>
        public async Task StartStep_EnterMyRound()
        {
            await GetCurrentPlayerHero().ActionManager.OnRequestStartStep_EnterMyRound();
        }

        /// <summary>
        /// 开始摸牌阶段
        /// </summary>
        /// <returns></returns>
        public async Task StartStep_PickCard()
        {
            await GetCurrentPlayerHero().ActionManager.OnRequestStartStep_PickCard();
        }

        /// <summary>
        /// 开始出牌阶段
        /// </summary>
        /// <returns></returns>
        public async Task StartStep_PlayCard()
        {
            await GetCurrentPlayerHero().ActionManager.OnRequestStartStep_PlayCard();
        }

        /// <summary>
        /// 开始弃牌阶段
        /// </summary>
        /// <returns></returns>
        public async Task StartStep_ThrowCard()
        {
            await GetCurrentPlayerHero().ActionManager.OnRequestStartStep_ThrowCard();
        }

        /// <summary>
        /// 开始回合结束阶段
        /// </summary>
        /// <returns></returns>
        public async Task StartStep_ExitMyRound()
        {
            await GetCurrentPlayerHero().ActionManager.OnRequestStartStep_ExitMyRound();
        }

        /// <summary>
        /// 当前player是否是主动出牌模式
        /// </summary>
        /// <returns></returns>
        public bool IsInZhudongMode()
        {
            return RoundContext != null;
        }

        /// <summary>
        /// 当前player是否是被动出牌模式
        /// </summary>
        /// <returns></returns>
        public bool IsInBeidongMode()
        {
            return CardRequestContext != null;
        }

        /// <summary>
        /// 当前player是否是空闲模式（不需要出牌，也不能出牌）
        /// </summary>
        /// <returns></returns>
        public bool IsInIdleMode()
        {
            return this.RoundContext == null && this.CardRequestContext == null;
        }

        /// <summary>
        /// 当前Player是否和targetPlayer同一阵营
        /// </summary>
        /// <param name="targetPlayer"></param>
        /// <returns></returns>
        public bool IsSameGroup(Player targetPlayer)
        {
            return GetCurrentPlayerHero().Hero.HeroGroup == targetPlayer.GetCurrentPlayerHero().Hero.HeroGroup;
        }

        /// <summary>
        /// 摸牌
        /// </summary>
        /// <param name="count"></param>
        public void PickCard(int count)
        {
            CardsInHand.AddRange(_gameLevel.PickNextCardsFromStack(count));
        }

        /// <summary>
        /// 弃牌
        /// </summary>
        /// <param name="cards"></param>
        public async Task ThrowCard(List<CardBase> cards)
        {
            //移除手牌中的对应牌
            CardsInHand.RemoveAll(c => cards.Any(m => m.CardId == c.CardId));
            //将弃掉的牌装入弃牌堆
            _gameLevel.ThrowCardToStack(cards);
            await Task.FromResult("");
        }

        /// <summary>
        /// 把当前牌移交给toPlayer
        /// </summary>
        /// <param name="toPlayer"></param>
        /// <param name="cards"></param>
        public async Task MoveCard(Player toPlayer, List<CardBase> cards)
        {
            //移除手牌中的对应牌
            CardsInHand.RemoveAll(c => cards.Any(m => m.CardId == c.CardId));
            //将弃掉的牌装目标手牌
            toPlayer.CardsInHand.AddRange(cards);
            await Task.FromResult("");
        }

        /// <summary>
        /// 给当前用户添加标记
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        public async Task AddMark(MarkBase mark)
        {
            var existMark = Marks.FirstOrDefault(p => p.MarkTypeId.Equals(mark.MarkTypeId));
            if (existMark != null && existMark.IsSummable())
            {
                existMark.MarkTimes++;
                return;
            }

            if (existMark != null)
            {
                return;
            }
            Marks.Add(mark);
            await Task.FromResult("");
        }

        /// <summary>
        /// 移除标记，如果是次数标记，则减去一次
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        public async Task RemoveMark(MarkBase mark)
        {
            var existMark = Marks.FirstOrDefault(p => p.MarkTypeId.Equals(mark.MarkTypeId));
            if (existMark != null)
            {
                if (existMark.IsSummable())
                {
                    existMark.MarkTimes--;
                    if (existMark.MarkTimes <= 0)
                    {
                        Marks.RemoveAll(p => p.MarkTypeId.Equals(mark.MarkTypeId));
                    }
                    return;
                }
                else
                {
                    Marks.RemoveAll(p => p.MarkTypeId.Equals(mark.MarkTypeId));
                }
            }
            else
            {
                throw new Exception("Unable to remove the mark, mark not found.");
            }
            await Task.FromResult("");
        }

        /// <summary>
        /// 移动Mark
        /// </summary>
        /// <param name="toPlayer"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public async Task MoveMark(Player toPlayer, MarkBase mark)
        {
            await RemoveMark(mark);
            await toPlayer.AddMark(mark);
        }

        /// <summary>
        /// 设置当前Player的上一个player
        /// </summary>
        /// <param name="player"></param>
        public void SetPreviousPlayer(Player player)
        {
            PreviousPlayer = player;
        }

        /// <summary>
        /// 获取下一个player，可以通过参数指定是否包含死亡的player
        /// </summary>
        /// <param name="includeDead">是否包好死亡的</param>
        /// <returns></returns>
        public Player GetNextPlayer(bool includeDead)
        {
            var next = NextPlayer;
            while (next.PlayerId != PlayerId)
            {
                if (includeDead)
                {
                    return next;
                }
                else
                {
                    //如果不包括死亡的且当前是死亡的，则继续查找
                    if (next.GetCurrentPlayerHero().IsDead)
                    {
                        next = next.NextPlayer;
                        continue;
                    }
                    else
                    {
                        return next;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 设置当前Player的下一个player
        /// </summary>
        /// <param name="player"></param>
        public void SetNextPlayer(Player player)
        {
            NextPlayer = player;
        }
    }
}
