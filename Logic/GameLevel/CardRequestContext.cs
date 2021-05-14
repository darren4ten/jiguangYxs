using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel.Panel;
using Logic.Model.Enums;
using Logic.Model.Player;

namespace Logic.GameLevel
{
    /// <summary>
    /// 请求出牌的上下文
    /// </summary>
    public class CardRequestContext
    {
        public Guid RequestId { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 最大出牌数
        /// </summary>
        public int MaxCardCountToPlay { get; set; }

        /// <summary>
        /// 最小出牌数
        /// </summary>
        public int MinCardCountToPlay { get; set; }

        /// <summary>
        /// 需要出的牌。如果是任意牌，则对应的CardType=any,RequestCard=null
        /// TODO: 优化类型，可否改为字符串或者枚举
        /// </summary>
        public CardBase RequestCard { get; set; }

        /// <summary>
        /// 卡牌范围
        /// </summary>
        public CardScopeEnum CardScope { get; set; }

        /// <summary>
        /// 花色要求
        /// </summary>
        public FlowerKindEnum FlowerKind { get; set; }

        /// <summary>
        /// 花色要求
        /// </summary>
        public AttackTypeEnum AttackType { get; set; }

        /// <summary>
        /// 攻击者
        /// </summary>
        public Player SrcPlayer { get; set; }

        /// <summary>
        /// 被攻击者
        /// </summary>
        public List<Player> TargetPlayers { get; set; }

        /// <summary>
        /// 用来攻击的来源牌
        /// </summary>
        public List<CardBase> SrcCards { get; set; }

        /// <summary>
        /// 攻击加成信息
        /// </summary>
        public AttackDynamicFactor AttackDynamicFactor { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否是群体请求
        /// </summary>
        public bool IsGroupRequest { get; set; }
        /// <summary>
        /// 是否是汇总的结果
        /// </summary>
        public bool IsMerged { get; set; }

        /// <summary>
        /// 需要摸牌的面板
        /// </summary>
        public PanelBase Panel { get; set; }

        /// <summary>
        /// 请求完成source
        /// </summary>
        public TaskCompletionSource<CardResponseContext> RequestTaskCompletionSource { get; set; }

        /// <summary>
        /// 备用的请求上下文，可以用来传递SelectTargetcontext
        /// </summary>
        public object AdditionalContext { get; set; }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public CardRequestContext DeepClone()
        {
            return new CardRequestContext()
            {
                RequestCard = RequestCard,
                MaxCardCountToPlay = MaxCardCountToPlay,
                AdditionalContext = AdditionalContext,
                AttackDynamicFactor = AttackDynamicFactor.DeepClone(),
                AttackType = AttackType,
                CardScope = CardScope,
                FlowerKind = FlowerKind,
                IsGroupRequest = IsGroupRequest,
                IsMerged = IsMerged,
                Message = Message,
                MinCardCountToPlay = MinCardCountToPlay,
                Panel = Panel,
                RequestId = RequestId,
                RequestTaskCompletionSource = RequestTaskCompletionSource,
                SrcCards = SrcCards,
                SrcPlayer = SrcPlayer,
                TargetPlayers = TargetPlayers
            };
        }

        /// <summary>
        /// 给定的卡牌是否符合request要求
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public bool IsMatch(IEnumerable<CardBase> cards)
        {
            if (cards == null)
            {
                return false;
            }

            if (IsCountMatch(cards) && cards.All(IsFlowKindMatch) && IsCardTypeMatch(cards))
            {
                return true;
            }

            return false;
        }

        public static CardRequestContext GetBaseCardRequestContext(List<Player> targets)
        {
            return new CardRequestContext()
            {
                MinCardCountToPlay = 1,
                MaxCardCountToPlay = 1,
                RequestId = Guid.NewGuid(),
                TargetPlayers = targets,
                AttackDynamicFactor = AttackDynamicFactor.GetDefaultBaseAttackFactor()
            };
        }

        private bool IsCardTypeMatch(IEnumerable<CardBase> cards)
        {
            return RequestCard == null || cards.All(p => p.GetType() == RequestCard.GetType());
        }

        private bool IsCountMatch(IEnumerable<CardBase> cards)
        {
            return cards != null && cards.Count() >= MinCardCountToPlay && cards.Count() <= MaxCardCountToPlay;
        }
        private bool IsFlowKindMatch(CardBase card)
        {
            return FlowerKind == FlowerKindEnum.Any || card.FlowerKind == FlowerKind;
        }
    }
}
