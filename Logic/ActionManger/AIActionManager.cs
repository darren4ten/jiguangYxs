using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.GameLevel.Panel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.EquipmentCards.Defense;
using Logic.Model.Cards.Interface;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Enums;
using Logic.Model.Mark;
using Logic.Model.RequestResponse.Request;
using Logic.Model.RequestResponse.Response;

namespace Logic.ActionManger
{
    /// <summary>
    /// AI行为管理器 
    /// </summary>
    public class AiActionManager : ActionManagerBase
    {
        public AiActionManager() { }

        public AiActionManager(PlayerContext playContext) : base(playContext)
        {
        }

        public override async Task<bool> OnRequestTriggerSkill(SkillTypeEnum skillType, CardRequestContext cardRequestContext)
        {
            if (skillType == SkillTypeEnum.Bolangchui)
            {
                return ShouldTriggerSkill_Bolangchui(cardRequestContext);
            }

            return await Task.FromResult(false);
        }

        /// <summary>
        /// 被动出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        public override async Task<CardResponseContext> OnRequestResponseCard(CardRequestContext cardRequestContext)
        {
            await Task.Delay(500);
            //任意牌，则检查手牌、装备牌是否可以出牌
            CardResponseContext response = null;
            //处理选择牌的请求
            if (cardRequestContext.AttackType == AttackTypeEnum.SelectCard)
            {
                return await OnRequestPickCardFromPanel(new PickCardFromPanelRequest()
                {
                    MaxCount = cardRequestContext.MaxCardCountToPlay,
                    MinCount = cardRequestContext.MinCardCountToPlay,
                    Panel = cardRequestContext.Panel,
                    RequestId = cardRequestContext.RequestId
                });
            }

            //处理其他类型
            if (cardRequestContext.CardType == CardTypeEnum.Any)
            {
                response = GetResponseCardByCardType_Any(cardRequestContext);
            }
            //非任意牌
            //1. 只允许手牌
            //2. 只允许武器牌
            //3. 只允许防具
            //4. 只允许装备牌
            else if (cardRequestContext.CardType == Enums.CardTypeEnum.InHand)
            {
                response = GetResponseCardByCardType_InHands(cardRequestContext);
            }
            else if (cardRequestContext.CardType == Enums.CardTypeEnum.InHandAndEquipment)
            {
                var exculdeEquipCards = new List<string>();
                if (cardRequestContext.AttackType == AttackTypeEnum.Bolangchui)
                {
                    exculdeEquipCards.Add(nameof(Bolangchui));
                }
                response = GetResponseCardByCardType_InHandsAmdEquipment(cardRequestContext, exculdeEquipCards);
            }
            else
            {
                throw new NotImplementedException($"{cardRequestContext.CardType}类型的请求未实现。");
            }
            //触发被动出牌事件
            return response;
        }

        public override async Task<CardResponseContext> OnRequestPickCardFromPanel(PickCardFromPanelRequest request)
        {
            //基本逻辑是
            //1. 如果选取的牌是来自友方，则优先抽取负面标记牌
            var response = new CardResponseContext()
            {
                Cards = new List<CardBase>() { }
            };
            if (request.Panel.CardOwner != null && request.Panel.CardOwner.IsSameGroup(PlayerContext.Player) && request.Panel.MarkCards != null)
            {
                var markCards = request.Panel.MarkCards.Where(p => p.Mark != null);
                //如果有画地为牢，则将其取掉
                var huadiweilaoMark = markCards.FirstOrDefault(p => nameof(p.Mark) == nameof(HuadiweilaoMark));
                if (huadiweilaoMark != null)
                {
                    //Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.GetCurrentPlayerHero().Hero.DisplayName}】从{request.Panel.CardOwner.PlayerId}的【{request.Panel.CardOwner.GetCurrentPlayerHero().Hero.DisplayName}】抽取了{huadiweilaoMark.Card.DisplayName}");
                    //request.Panel.CardOwner.Marks.Remove(huadiweilaoMark.Mark);
                    huadiweilaoMark.SelectedBy = PlayerContext.Player;
                    response.Cards.Add(huadiweilaoMark.Card);
                }
            }

            if (response.Cards.Count >= request.MaxCount)
            {
                return response;
            }

            //2. 如果有装备牌，则选择装备牌。优先级从高到低是武器、防具、防御马、进攻马
            if (request.Panel.EquipmentCards?.Any() == true)
            {
                //判断是否有武器
                await RemoveEquipment<IWeapon>(response, request.Panel);
                if (response.Cards.Count >= request.MaxCount)
                {
                    return response;
                }

                //判断是否有防具
                await RemoveEquipment<IDefender>(response, request.Panel);
                if (response.Cards.Count >= request.MaxCount)
                {
                    return response;
                }

                //判断是否有防御马
                await RemoveEquipment<Fangyuma>(response, request.Panel);
                if (response.Cards.Count >= request.MaxCount)
                {
                    return response;
                }

                //判断是否有进攻马
                await RemoveEquipment<Jingongma>(response, request.Panel);
                if (response.Cards.Count >= request.MaxCount)
                {
                    return response;
                }
            }

            //3. 如果有手牌，则选择手牌
            if (request.Panel.InHandCards?.Any() == true)
            {
                var maxCount = request.MaxCount - response.Cards.Count;
                var takeCards = request.Panel.InHandCards.Take(maxCount);
                response.Cards.AddRange(takeCards.Select(t =>
                {
                    t.SelectedBy = PlayerContext.Player;
                    return t.Card;
                }));
            }
            return response;
        }

        public override async Task OnRequestStartStep_EnterMyRound()
        {
            throw new NotImplementedException();
        }

        public override async Task OnRequestStartStep_PickCard()
        {
            throw new NotImplementedException();
        }

        public override async Task OnRequestStartStep_PlayCard()
        {
            throw new NotImplementedException();
        }

        public override async Task OnRequestStartStep_ThrowCard()
        {
            throw new NotImplementedException();
        }

        public override async Task OnRequestStartStep_ExitMyRound()
        {
            throw new NotImplementedException();
        }

        public override async Task<SelectedTargetsResponse> OnRequestSelectTargets(SelectedTargetsRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取Card的API值
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public AiValue GetCardAiValue(CardBase card)
        {
            var valueDic = new Dictionary<string, AiValue>()
            {
                { nameof(Sha),new AiValue()
                {
                    Priority = 120,
                    Value = 200
                }},
                { nameof(Juedou),new AiValue(){
                    Priority = 150,
                    Value = 200
                }},
                { nameof(Yao),new AiValue(){
                    Priority = 175,
                    Value = 250
                }},
                { nameof(Fenghuolangyan),new AiValue(){
                    Priority = 170,
                    Value = 100
                }},
                { nameof(Wanjianqifa),new AiValue(){
                    Priority = 170,
                    Value = 100
                }},
                { nameof(Jiedaosharen),new AiValue(){
                    Priority = 190,
                    Value = 80
                }},
                { nameof(Fudichouxin),new AiValue(){
                    Priority = 200,
                    Value = 200
                }},
                { nameof(Tannangquwu),new AiValue(){
                    Priority = 180,
                    Value = 220
                }},
                { nameof(Huadiweilao),new AiValue(){
                    Priority = 100,
                    Value = 220
                }},
                { nameof(Luyeqiang),new AiValue(){
                    Priority = 160,
                    Value = 150
                }},
                { nameof(Hufu),new AiValue(){
                    Priority = 100,
                    Value = 230
                }},
                { nameof(Longlindao),new AiValue(){
                    Priority = 160,
                    Value = 80
                }},
                { nameof(Langyabang),new AiValue(){
                    Priority = 150,
                    Value = 80
                }},
                { nameof(Bawanggong),new AiValue(){
                    Priority = 160,
                    Value = 80
                }},
                { nameof(Panlonggun),new AiValue(){
                    Priority = 150,
                    Value = 80
                }},
                { nameof(Yuruyi),new AiValue(){
                    Priority = 160,
                    Value = 190
                }},
            };

            return valueDic.ContainsKey(nameof(card)) ? valueDic[nameof(card)] : new AiValue()
            {
                Priority = 100,
                Value = 100
            };
        }

        #region 私有方法

        #region 博浪锤技能

        /// <summary>
        /// 是否要发动博浪锤.
        /// 发动条件：
        ///     1. 被攻击的对象是敌方
        ///     2. 且本人手牌和装备牌中有至少两张手牌（除博浪锤之外）
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private bool ShouldTriggerSkill_Bolangchui(CardRequestContext cardRequestContext)
        {
            var target = cardRequestContext.TargetPlayers.FirstOrDefault();
            if (target == null)
            {
                throw new Exception("攻击目标不能为空。");
            }

            if (target.IsSameGroup(cardRequestContext.SrcPlayer))
            {
                return false;
            }

            var totalCount = cardRequestContext.SrcPlayer.CardsInHand.Count +
                 cardRequestContext.SrcPlayer.EquipmentSet.Count(p => !(p is Bolangchui));
            if (totalCount >= 2)
            {
                return true;
            }
            return false;
        }

        #endregion

        private async Task RemoveEquipment<T>(CardResponseContext responseContext, PanelBase panel)
        {
            //如果有画地为牢，则将其取掉
            var equipment = panel.EquipmentCards.FirstOrDefault(p => p.Card is T);
            if (equipment != null)
            {
                //Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.GetCurrentPlayerHero().Hero.DisplayName}】从{panel.CardOwner.PlayerId}的【{panel.CardOwner.GetCurrentPlayerHero().Hero.DisplayName}】抽取了{equipment.Card.DisplayName}");
                //await panel.CardOwner.RemoveEquipment(equipment.Card, null, null, null);
                equipment.SelectedBy = PlayerContext.Player;
                responseContext.Cards.Add(equipment.Card);
            }

            await Task.FromResult(0);
        }

        /// <summary>
        /// 处理从手牌、装备区中出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="excludeEquipCards">要排除的卡类型(装备区)</param>
        /// <returns></returns>
        private CardResponseContext GetResponseCardByCardType_InHandsAmdEquipment(CardRequestContext cardRequestContext, List<string> excludeEquipCards)
        {
            var sumCards = new List<CardBase>(PlayerContext.Player.CardsInHand);
            sumCards.AddRange(excludeEquipCards == null ? PlayerContext.Player.EquipmentSet : PlayerContext.Player.EquipmentSet.Where(p => excludeEquipCards.All(e => !p.Name.Equals(e))));
            var cardsToPlay = sumCards;
            //没有指定出什么牌，则返回任意牌
            if (cardRequestContext.RequestCard != null)
            {
                cardsToPlay = sumCards.Where(s => s.GetType() == cardRequestContext.RequestCard.GetType()).ToList();
            }

            var minCount = cardRequestContext.MinCardCountToPlay;
            var maxCount = cardRequestContext.MaxCardCountToPlay;
            cardsToPlay = GetCardsOrderByAiValue(minCount, maxCount,
                cardsToPlay, true);
            return new CardResponseContext()
            {
                Cards = cardsToPlay.ToList(),
            };
        }

        /// <summary>
        /// 处理从手牌中出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private CardResponseContext GetResponseCardByCardType_InHands(CardRequestContext cardRequestContext)
        {
            var sumCards = new List<CardBase>(PlayerContext.Player.CardsInHand);
            var cardsToPlay = sumCards;
            //没有指定出什么牌，则返回任意牌
            if (cardRequestContext.RequestCard != null)
            {
                cardsToPlay = sumCards.Where(s => s.GetType() == cardRequestContext.RequestCard.GetType()).ToList();
            }

            var minCount = cardRequestContext.MinCardCountToPlay;
            var maxCount = cardRequestContext.MaxCardCountToPlay;
            cardsToPlay = GetCardsOrderByAiValue(minCount, maxCount,
                cardsToPlay, true);
            return new CardResponseContext()
            {
                Cards = cardsToPlay.ToList(),
            };
        }

        /// <summary>
        /// 处理“任意”类型的请求
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private CardResponseContext GetResponseCardByCardType_Any(CardRequestContext cardRequestContext)
        {
            var sumCards = new List<CardBase>(PlayerContext.Player.CardsInHand);
            //返回一张价值最低的牌
            if (PlayerContext.Player.EquipmentSet != null)
            {
                sumCards.AddRange(PlayerContext.Player.EquipmentSet);
            }

            var cardsToPlay = sumCards;
            //没有指定出什么牌，则返回任意牌
            if (cardRequestContext.RequestCard != null)
            {
                cardsToPlay = sumCards.Where(s => s.GetType() == cardRequestContext.RequestCard.GetType()).ToList();
            }

            var minCount = cardRequestContext.MinCardCountToPlay;
            var maxCount = cardRequestContext.MaxCardCountToPlay;
            cardsToPlay = GetCardsOrderByAiValue(minCount, maxCount,
                cardsToPlay, true);
            return new CardResponseContext()
            {
                Cards = cardsToPlay?.ToList(),
            };
        }

        /// <summary>
        /// 按照AI value由低到高取开
        /// </summary>
        /// <param name="minCount"></param>
        /// <param name="maxCount"></param>
        /// <param name="cards"></param>
        /// <param name="asc">ai value 升序</param>
        /// <returns></returns>
        private List<CardBase> GetCardsOrderByAiValue(int minCount, int maxCount, List<CardBase> cards, bool asc)
        {
            if (cards.Count >= minCount )
            {
                var avCards = asc ? cards.OrderBy(p => GetCardAiValue(p).Value) : cards.OrderByDescending(p => GetCardAiValue(p).Value);
                var cardsToPlay = avCards.Take(maxCount);
                return cardsToPlay.ToList();
            }

            //不够就不出
            return null;
        }
        #endregion

    }

    public class AiValue
    {
        public int Priority { get; set; }
        public double Value { get; set; }
    }
}
