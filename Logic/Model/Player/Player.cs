﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.Cards;
using Logic.Event;
using Logic.GameLevel;
using Logic.Model.Cards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.Interface;
using Logic.Model.Mark;
using Logic.Model.Skill.Interface;
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
        /// 玩家所在阵营id，用来标记各个player之间是否为敌对关系
        /// </summary>
        public Guid GroupId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 是否死亡
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return _availablePlayerHeroes.Any(p => !p.IsDead);
        }

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

        public int ExtraXianshou { get; set; }

        public PlayerUIState PlayerUiState { get; }

        #region 牌
        /// <summary>
        /// 手牌
        /// </summary>
        public List<CardBase> CardsInHand { get; private set; }

        /// <summary>
        /// 玩家当前的装备牌
        /// </summary>
        public List<CardBase> EquipmentSet { get; private set; }

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
        /// 用户行为管理器
        /// </summary>
        public ActionManagerBase ActionManager { get; }

        /// <summary>
        /// 后一个玩家
        /// </summary>
        public Player NextPlayer { get; private set; }

        #endregion

        public Player(GameLevelBase gameLevel, ActionManagerBase actionManager, List<PlayerHero> availablePlayerHeroes)
        {

            _gameLevel = gameLevel;
            ActionManager = actionManager;
            _availablePlayerHeroes = availablePlayerHeroes;
            CardsInHand = new List<CardBase>();
            Marks = new List<Mark.MarkBase>();
            EquipmentSet = new List<CardBase>();
            PlayerUiState = new PlayerUIState(this);
        }

        public void Init()
        {
            var playerContext = new PlayerContext()
            {
                GameLevel = _gameLevel,
                Player = this
            };
            ActionManager.SetPlayerContext(playerContext);
            _availablePlayerHeroes.ForEach(p =>
            {
                p.AttachPlayerContext(playerContext);
            });

            var curHero = GetCurrentPlayerHero();
            curHero.SetupSkills();
        }

        /// <summary>
        /// 获取先手值
        /// </summary>
        /// <returns></returns>
        public int GetXianshou()
        {
            return GetCurrentPlayerHero().GetXianshou() + ExtraXianshou;
        }

        /// <summary>
        /// 当前用户英雄
        /// </summary>
        public PlayerHero GetCurrentPlayerHero()
        {
            return _availablePlayerHeroes.OrderByDescending(p => p.IsDead).First();
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
        public async Task<CardResponseContext> ResponseCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            cardRequestContext.AttackDynamicFactor =
                cardRequestContext.AttackDynamicFactor ?? AttackDynamicFactor.GetDefaultDeltaAttackFactor();
            //设置被请求的上下文
            CardRequestContext = cardRequestContext;
            var responseContext = new CardResponseContext();
            await TriggerEvent(Enums.EventTypeEnum.BeforeBeidongPlayCard, cardRequestContext, responseContext, roundContext);
            await TriggerEvent(Enums.EventTypeEnum.BeidongPlayCard, cardRequestContext, responseContext, roundContext);
            //如果被取消或者处理成功了，就直接返回
            if (responseContext.ResponseResult == ResponseResultEnum.Success || responseContext.ResponseResult == ResponseResultEnum.Cancelled)
            {
                return responseContext;
            }
            var newRequestContext = GetCombindCardRequestContext(cardRequestContext,
                GetCurrentPlayerHero().BaseAttackFactor, roundContext);
            var res = await ActionManager.OnRequestResponseCard(newRequestContext);
            await TriggerEvent(Enums.EventTypeEnum.AfterBeidongPlayCard, cardRequestContext, res, roundContext);
            if (res.Cards?.Any() == true)
            {
                string actionName = cardRequestContext.AttackType == AttackTypeEnum.SelectCard ? "选择了" : "出牌";
                Console.WriteLine($"[{PlayerName}{PlayerId}]{actionName}{string.Join(",", res.Cards.Select(p => p.ToString()))}");
                //将牌置于临时牌堆
                res.Cards.ForEach(async c =>
                {
                    //是装备牌
                    if (EquipmentSet.Any(p => p == c))
                    {
                        await RemoveEquipment(c, null, null, null);
                    }
                    else
                    {
                        await RemoveCardsInHand(new List<CardBase>() { c }, null, null, null);
                    }
                });
                _gameLevel.TempCardDesk.Add(res.Cards);
            }
            //清除被请求的上下文
            CardRequestContext = null;
            return res;
        }

        /// <summary>
        /// 触发当前Player的事件，会引用修改所有的参数，如cardRequestContext，responseContext
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="cardRequestContext"></param>
        /// <param name="responseContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        public async Task<CardResponseContext> TriggerEvent(Enums.EventTypeEnum eventType, CardRequestContext cardRequestContext, CardResponseContext responseContext, RoundContext roundContext = null)
        {
            await _gameLevel.GlobalEventBus.TriggerEvent(eventType, this.GetCurrentPlayerHero(), cardRequestContext, roundContext, responseContext);
            return responseContext;
        }

        /// <summary>
        /// 监听当前Player的事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="cardRequestContext"></param>
        /// <param name="responseContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        public void ListenEvent(Guid eventId, Enums.EventTypeEnum eventType, EventBus.RoundEventHandler handler)
        {
            _gameLevel.GlobalEventBus.ListenEvent(eventId, this.GetCurrentPlayerHero(), eventType, handler);
        }

        public async Task StartMyRound()
        {
            await StartStep_EnterMyRound();
            await StartStep_PickCard();
            await StartStep_PlayCard();
            await StartStep_ThrowCard();
            await StartStep_ExitMyRound();
        }

        /// <summary>
        /// 开始进入我的回合
        /// </summary>
        /// <returns></returns>
        public async Task StartStep_EnterMyRound()
        {
            RoundContext = new RoundContext()
            {
                AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor(),
                SkillTriggerTimesDic = new Dictionary<SkillTypeEnum, int>()
            };
            var request = new CardRequestContext();
            var response = new CardResponseContext();
            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeEnterMyRound, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.BeforeEnterMyRound, request, response, RoundContext);

            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.EnterMyRound, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.EnterMyRound, request, response, RoundContext);
            var combinedRequest = GetCombindCardRequestContext(request, GetCurrentPlayerHero().BaseAttackFactor, RoundContext);
            if (combinedRequest.AttackDynamicFactor.SkipOption.ShouldSkipEnterMyRound)
            {
                Console.WriteLine($"跳过进入我的回合阶段。");
                return;
            }

            await ActionManager.OnRequestStartStep_EnterMyRound();
            Console.WriteLine($"进入{PlayerId}【{GetCurrentPlayerHero().Hero.DisplayName}】的回合阶段。");
            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterEnterMyRound, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.AfterEnterMyRound, request, response, RoundContext);
        }

        /// <summary>
        /// 开始摸牌阶段
        /// </summary>
        /// <returns></returns>
        public async Task StartStep_PickCard()
        {
            var request = new CardRequestContext();
            var response = new CardResponseContext();
            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforePickCard, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.BeforePickCard, request, response, RoundContext);

            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.PickCard, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.PickCard, request, response, RoundContext);
            var combinedRequest = GetCombindCardRequestContext(request, GetCurrentPlayerHero().BaseAttackFactor, RoundContext);
            if (combinedRequest.AttackDynamicFactor.SkipOption.ShouldSkipPickCard)
            {
                Console.WriteLine($"跳过摸牌。");
                return;
            }

            await ActionManager.OnRequestStartStep_PickCard();
            await PickCard(combinedRequest.AttackDynamicFactor.PickCardCountPerRound);
            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterPickCard, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.AfterPickCard, request, response, RoundContext);
        }

        /// <summary>
        /// 开始出牌阶段
        /// </summary>
        /// <returns></returns>
        public async Task StartStep_PlayCard()
        {
            if (RoundContext.AttackDynamicFactor.SkipOption.ShouldSkipPlayCard)
            {
                Console.WriteLine($"跳过出牌。");
                return;
            }
            var request = new CardRequestContext();
            var response = new CardResponseContext();
            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeZhudongPlayCard, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.BeforeZhudongPlayCard, request, response, RoundContext);

            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.ZhudongPlayCard, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.ZhudongPlayCard, request, response, RoundContext);
            var combinedRequest = GetCombindCardRequestContext(request, GetCurrentPlayerHero().BaseAttackFactor, RoundContext);
            if (combinedRequest.AttackDynamicFactor.SkipOption.ShouldSkipPlayCard)
            {
                Console.WriteLine($"跳过出牌。");
                return;
            }

            await ActionManager.OnRequestStartStep_PlayCard();

            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterZhudongPlayCard, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.AfterZhudongPlayCard, request, response, RoundContext);

        }

        /// <summary>
        /// 开始弃牌阶段
        /// </summary>
        /// <returns></returns>
        public async Task StartStep_ThrowCard()
        {
            var request = new CardRequestContext();
            var response = new CardResponseContext();
            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeThrowCard, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.BeforeThrowCard, request, response, RoundContext);

            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.ThrowCard, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.ThrowCard, request, response, RoundContext);
            var combinedRequest = GetCombindCardRequestContext(request, GetCurrentPlayerHero().BaseAttackFactor, RoundContext);
            if (combinedRequest.AttackDynamicFactor.SkipOption.ShouldSkipThrowCard)
            {
                Console.WriteLine($"跳过弃牌。");
                return;
            }

            await ActionManager.OnRequestStartStep_ThrowCard();

            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterThrowCard, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.AfterThrowCard, request, response, RoundContext);
        }

        /// <summary>
        /// 开始回合结束阶段
        /// </summary>
        /// <returns></returns>
        public async Task StartStep_ExitMyRound()
        {
            var request = new CardRequestContext();
            var response = new CardResponseContext();
            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeEndRound, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.BeforeEndRound, request, response, RoundContext);

            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.EndRound, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.EndRound, request, response, RoundContext);
            var combinedRequest = GetCombindCardRequestContext(request, GetCurrentPlayerHero().BaseAttackFactor, RoundContext);
            if (combinedRequest.AttackDynamicFactor.SkipOption.ShouldSkipExitMyRound)
            {
                Console.WriteLine($"跳过结束出牌阶段。");
                return;
            }

            await ActionManager.OnRequestStartStep_ExitMyRound();
            await _gameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterEndRound, _gameLevel.HostPlayerHero,
                request, RoundContext, response);
            await TriggerEvent(EventTypeEnum.AfterEndRound, request, response, RoundContext);
        }

        /// <summary>
        /// 将牌放入玩家手中
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public async Task AddCardsInHand(IEnumerable<CardBase> cards)
        {
            CardsInHand.AddRange(cards.Select(c => c.AttachPlayerContext(new PlayerContext() { GameLevel = _gameLevel, Player = this })));
            await Task.FromResult(0);
        }

        /// <summary>
        /// 将牌放入玩家手中
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public async Task AddCardInHand(CardBase card)
        {
            CardsInHand.Add(card.AttachPlayerContext(new PlayerContext() { GameLevel = _gameLevel, Player = this }));
            await Task.FromResult(0);
        }

        /// <summary>
        /// 移除手牌，会触发事件
        /// </summary>
        /// <param name="equipmentCard"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        public async Task RemoveCardsInHand(List<CardBase> cards, CardRequestContext request, CardResponseContext response, RoundContext roundContext)
        {
            if (cards == null)
            {
                return;
            }

            await TriggerEvent(EventTypeEnum.BeforeLoseCardsInHand, request, response, roundContext);
            await TriggerEvent(EventTypeEnum.LoseCardsInHand, request, response, roundContext);
            cards.ForEach(c =>
            {
                CardsInHand.Remove(c);
                _gameLevel.TempCardDesk.Add(c);
            });
            await TriggerEvent(EventTypeEnum.AfterLoseCardsInHand, request, response, roundContext);
        }

        /// <summary>
        /// 移除装备栏中的装备。会触发移除装备的事件
        /// </summary>
        /// <param name="equipmentCard"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        public async Task RemoveEquipment(CardBase equipmentCard, CardRequestContext request, CardResponseContext response, RoundContext roundContext)
        {
            if (EquipmentSet == null)
            {
                return;
            }

            var equip = equipmentCard as EquipmentBase;
            if (equip == null)
            {
                Console.WriteLine($"移除装备失败，该牌不是装备牌：{equipmentCard.DisplayName}");
                return;
            }

            var removedEq = EquipmentSet.Remove(equipmentCard);
            await equip.UnEquip();
            //触发卸载装备的事件
            Console.WriteLine($"移除装备{equip.DisplayName}{(removedEq ? "成功!" : "失败!!!")}");
        }

        /// <summary>
        /// 装备装备
        /// </summary>
        /// <param name="equipmentCard"></param>
        /// <returns></returns>
        public async Task AddEquipment(CardBase equipmentCard)
        {
            if (!(equipmentCard is EquipmentBase))
            {
                Console.WriteLine($"装备{equipmentCard.DisplayName}失败，不是装备。");
                return;
            }

            if (equipmentCard.PlayerContext == null)
            {
                equipmentCard.AttachPlayerContext(new PlayerContext()
                {
                    Player = this,
                    GameLevel = _gameLevel
                });
            }
            var hasEquiped = await EquipEquipment<IWeapon>(equipmentCard);
            if (hasEquiped)
            {
                return;
            }
            hasEquiped = await EquipEquipment<IDefender>(equipmentCard);

            if (hasEquiped)
            {
                return;
            }
            hasEquiped = await EquipEquipment<Jingongma>(equipmentCard);

            if (hasEquiped)
            {
                return;
            }
            await EquipEquipment<Fangyuma>(equipmentCard);
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
        /// 获取所有的技能按钮
        /// </summary>
        /// <returns></returns>
        public List<ISkillButton> GetAllSkillButtons()
        {
            var skillBtnInfos = new List<ISkillButton>();
            var equipSkilBtns = EquipmentSet?.OfType<ISkillButton>();
            if (equipSkilBtns != null)
            {
                skillBtnInfos.AddRange(equipSkilBtns);
            }
            var skillBtns = GetCurrentPlayerHero().GetAllMainSkills().OfType<ISkillButton>();

            skillBtnInfos.AddRange(skillBtns);
            return skillBtnInfos;
        }

        /// <summary>
        /// 当前Player是否和targetPlayer同一阵营
        /// </summary>
        /// <param name="targetPlayer"></param>
        /// <returns></returns>
        public bool IsSameGroup(Player targetPlayer)
        {
            //return GetCurrentPlayerHero().Hero.HeroGroup == targetPlayer.GetCurrentPlayerHero().Hero.HeroGroup;
            return GroupId == targetPlayer.GroupId;
        }

        /// <summary>
        /// 摸牌
        /// </summary>
        /// <param name="count"></param>
        public async Task PickCard(int count)
        {
            await AddCardsInHand(_gameLevel.PickNextCardsFromStack(count));
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
            mark.PlayerContext = new PlayerContext()
            {
                GameLevel = _gameLevel,
                Player = this
            };
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
            mark.Init();
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
                mark.Reset();
                mark.PlayerContext = null;
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
                        //如果下一个player还是自己，则直接返回null
                        return next == this ? null : next;
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

        /// <summary>
        /// 叠加计算各个因子
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="baseAttackDynamicFactor"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        public CardRequestContext GetCombindCardRequestContext(CardRequestContext cardRequestContext, AttackDynamicFactor baseAttackDynamicFactor, RoundContext roundContext)
        {
            if (cardRequestContext == null)
            {
                return null;
            }
            var newCardRequestContext = new CardRequestContext()
            {
                CardScope = cardRequestContext.CardScope,
                AttackType = cardRequestContext.AttackType,
                FlowerKind = cardRequestContext.FlowerKind,
                MaxCardCountToPlay = cardRequestContext.MaxCardCountToPlay,
                MinCardCountToPlay = cardRequestContext.MinCardCountToPlay,
                Message = cardRequestContext.Message,
                RequestCard = cardRequestContext.RequestCard,
                SrcCards = cardRequestContext.SrcCards,
                SrcPlayer = cardRequestContext.SrcPlayer,
                TargetPlayers = cardRequestContext.TargetPlayers,
                IsMerged = true,
                Panel = cardRequestContext.Panel
            };
            newCardRequestContext.AttackDynamicFactor = DeepCloneAttackDynamicFactor(cardRequestContext.AttackDynamicFactor);
            if (baseAttackDynamicFactor != null)
            {
                newCardRequestContext.AttackDynamicFactor =
                    MergeAttackDynamicFactor(newCardRequestContext.AttackDynamicFactor, baseAttackDynamicFactor);
            }

            if (roundContext != null)
            {
                newCardRequestContext.AttackDynamicFactor =
                    MergeAttackDynamicFactor(newCardRequestContext.AttackDynamicFactor, roundContext.AttackDynamicFactor);
            }

            return newCardRequestContext;
        }

        #region 私有方法
        /// <summary>
        /// 根据指定的类型装备武器,如果装备成功/异常，则返回true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="equipmentCard"></param>
        /// <returns></returns>
        private async Task<bool> EquipEquipment<T>(CardBase equipmentCard)
        {
            var equipment = equipmentCard as EquipmentBase;
            if (equipmentCard is T)
            {
                var exist = EquipmentSet.FirstOrDefault(p => p is T);
                if (exist != null)
                {
                    EquipmentSet.Remove(exist);
                    await (exist as EquipmentBase).UnEquip();
                }
                EquipmentSet.Add(equipmentCard);
                await equipment.Equip();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <param name="factor"></param>
        /// <returns></returns>
        private AttackDynamicFactor DeepCloneAttackDynamicFactor(AttackDynamicFactor factor)
        {
            var newAttackDynamicFactor = factor == null ? AttackDynamicFactor.GetDefaultDeltaAttackFactor() :
                new AttackDynamicFactor()
                {
                    DefenseDistance = factor.DefenseDistance,
                    TannangDistance = factor.TannangDistance,
                    IsShaNotAvoidable = factor.IsShaNotAvoidable,
                    MaxCardCountInHand = factor.MaxCardCountInHand,
                    WuzhongshengyouCardCount = factor.WuzhongshengyouCardCount,
                    MaxLife = factor.MaxLife,
                    MaxShaTargetCount = factor.MaxShaTargetCount,
                    MaxShaTimes = factor.MaxShaTimes,
                    PickCardCountPerRound = factor.PickCardCountPerRound,
                    ShaCountAvoidJuedou = factor.ShaCountAvoidJuedou,
                    ShaDistance = factor.ShaDistance,
                    ShanCountAvoidSha = factor.ShanCountAvoidSha,
                    Damage = new Damage()
                    {
                        FenghuolangyanDamage = factor.Damage.FenghuolangyanDamage,
                        JuedouDamage = factor.Damage.JuedouDamage,
                        ShaDamage = factor.Damage.ShaDamage,
                        WanjianqifaDamage = factor.Damage.WanjianqifaDamage,
                        ShoupengleiDamage = factor.Damage.ShoupengleiDamage,
                        GongxinDamage = factor.Damage.GongxinDamage,
                        DujiDamage = factor.Damage.DujiDamage
                    },
                    Recover = new Recover()
                    {
                        XiuyangshengxiLife = factor.Recover.XiuyangshengxiLife,
                        XixueLife = factor.Recover.XixueLife,
                        YaoLife = factor.Recover.YaoLife
                    },
                    SkipOption = new SkipOption()
                    {
                        ShouldSkipEnterMyRound = factor.SkipOption.ShouldSkipEnterMyRound,
                        ShouldSkipPickCard = factor.SkipOption.ShouldSkipPickCard,
                        ShouldSkipPlayCard = factor.SkipOption.ShouldSkipPlayCard,
                        ShouldSkipThrowCard = factor.SkipOption.ShouldSkipThrowCard,
                    }
                };
            return newAttackDynamicFactor;
        }

        /// <summary>
        /// 将两个AttackDynamicFactor参数合并
        /// </summary>
        /// <param name="src"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public AttackDynamicFactor MergeAttackDynamicFactor(AttackDynamicFactor src, AttackDynamicFactor target)
        {
            if (src == null)
            {
                return DeepCloneAttackDynamicFactor(target);
            }

            if (target == null)
            {
                return DeepCloneAttackDynamicFactor(src);
            }
            AttackDynamicFactor result = AttackDynamicFactor.GetDefaultDeltaAttackFactor();

            result.MaxLife += src.MaxLife;
            result.DefenseDistance += src.DefenseDistance;
            result.IsShaNotAvoidable = result.IsShaNotAvoidable || src.IsShaNotAvoidable;
            result.IsShaNotAvoidableByYuruyi = result.IsShaNotAvoidableByYuruyi || src.IsShaNotAvoidableByYuruyi;
            result.MaxCardCountInHand += src.MaxCardCountInHand;
            result.MaxShaTargetCount += src.MaxShaTargetCount;
            result.MaxShaTimes += src.MaxShaTimes;
            result.PickCardCountPerRound += src.PickCardCountPerRound;
            result.ShaCountAvoidJuedou += src.ShaCountAvoidJuedou;
            result.ShaDistance += src.ShaDistance;
            result.ShanCountAvoidSha += src.ShanCountAvoidSha;
            result.TannangDistance += src.TannangDistance;
            result.WuzhongshengyouCardCount += src.WuzhongshengyouCardCount;

            result.SkipOption.ShouldSkipEnterMyRound = result.SkipOption.ShouldSkipEnterMyRound || src.SkipOption.ShouldSkipEnterMyRound;
            result.SkipOption.ShouldSkipPickCard = result.SkipOption.ShouldSkipPickCard || src.SkipOption.ShouldSkipPickCard;
            result.SkipOption.ShouldSkipPlayCard = result.SkipOption.ShouldSkipPlayCard || src.SkipOption.ShouldSkipPlayCard;
            result.SkipOption.ShouldSkipThrowCard = result.SkipOption.ShouldSkipThrowCard || src.SkipOption.ShouldSkipThrowCard;

            result.Recover.XiuyangshengxiLife += src.Recover?.XiuyangshengxiLife ?? 0;
            result.Recover.XixueLife += src.Recover?.XixueLife ?? 0;
            result.Recover.YaoLife += src.Recover?.YaoLife ?? 0;

            result.Damage.ShaDamage += src.Damage?.ShaDamage ?? 0;
            result.Damage.DujiDamage += src.Damage?.DujiDamage ?? 0;
            result.Damage.FenghuolangyanDamage += src.Damage?.FenghuolangyanDamage ?? 0;
            result.Damage.GongxinDamage += src.Damage?.GongxinDamage ?? 0;
            result.Damage.JuedouDamage += src.Damage?.JuedouDamage ?? 0;
            result.Damage.WanjianqifaDamage += src.Damage?.WanjianqifaDamage ?? 0;
            result.Damage.ShoupengleiDamage += src.Damage?.ShoupengleiDamage ?? 0;

            result.MaxLife += target.MaxLife;
            result.DefenseDistance += target.DefenseDistance;
            result.IsShaNotAvoidable = result.IsShaNotAvoidable || target.IsShaNotAvoidable;
            result.IsShaNotAvoidableByYuruyi = result.IsShaNotAvoidableByYuruyi || target.IsShaNotAvoidableByYuruyi;
            result.MaxCardCountInHand += target.MaxCardCountInHand;
            result.MaxShaTargetCount += target.MaxShaTargetCount;
            result.MaxShaTimes += target.MaxShaTimes;
            result.PickCardCountPerRound += target.PickCardCountPerRound;
            result.ShaCountAvoidJuedou += target.ShaCountAvoidJuedou;
            result.ShaDistance += target.ShaDistance;
            result.ShanCountAvoidSha += target.ShanCountAvoidSha;
            result.TannangDistance += target.TannangDistance;
            result.WuzhongshengyouCardCount += target.WuzhongshengyouCardCount;

            result.SkipOption.ShouldSkipEnterMyRound = result.SkipOption.ShouldSkipEnterMyRound || target.SkipOption.ShouldSkipEnterMyRound;
            result.SkipOption.ShouldSkipPickCard = result.SkipOption.ShouldSkipPickCard || target.SkipOption.ShouldSkipPickCard;
            result.SkipOption.ShouldSkipPlayCard = result.SkipOption.ShouldSkipPlayCard || target.SkipOption.ShouldSkipPlayCard;
            result.SkipOption.ShouldSkipThrowCard = result.SkipOption.ShouldSkipThrowCard || target.SkipOption.ShouldSkipThrowCard;

            result.Recover.XiuyangshengxiLife += target.Recover?.XiuyangshengxiLife ?? 0;
            result.Recover.XixueLife += target.Recover?.XixueLife ?? 0;
            result.Recover.YaoLife += target.Recover?.YaoLife ?? 0;

            result.Damage.ShaDamage += target.Damage?.ShaDamage ?? 0;
            result.Damage.DujiDamage += target.Damage?.DujiDamage ?? 0;
            result.Damage.FenghuolangyanDamage += target.Damage?.FenghuolangyanDamage ?? 0;
            result.Damage.GongxinDamage += target.Damage?.GongxinDamage ?? 0;
            result.Damage.JuedouDamage += target.Damage?.JuedouDamage ?? 0;
            result.Damage.WanjianqifaDamage += target.Damage?.WanjianqifaDamage ?? 0;
            result.Damage.ShoupengleiDamage += target.Damage?.ShoupengleiDamage ?? 0;
            return result;
        }


        #endregion
    }
}
