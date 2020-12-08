using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Cards.EquipmentCards.Defense;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Enums;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 被动出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        public override async Task<CardResponseContext> OnRequestResponseCard(CardRequestContext cardRequestContext)
        {
            await Task.Delay(500);
            //任意牌，则检查手牌、装备牌是否可以出牌
            CardResponseContext response = null;
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
            else
            {
                throw new NotImplementedException($"{cardRequestContext.CardType}类型的请求未实现。");
            }
            //触发被动出牌事件
            return response;
        }

        public override async Task<CardResponseContext> OnRequestPickCardFromPanel(PickCardFromPanelRequest request)
        {
            throw new NotImplementedException($"");
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
                { nameof(Sha),new AiValue(){
                    Priority = 120,
                    Value = 200
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

            cardsToPlay = GetCardsOrderByAiValue(cardRequestContext.MinCardCountToPlay, cardRequestContext.MaxCardCountToPlay,
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

            cardsToPlay = GetCardsOrderByAiValue(cardRequestContext.MinCardCountToPlay, cardRequestContext.MaxCardCountToPlay,
                cardsToPlay, true);
            return new CardResponseContext()
            {
                Cards = cardsToPlay.ToList(),
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
            if (cards.Count >= minCount && cards.Count <= maxCount)
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
