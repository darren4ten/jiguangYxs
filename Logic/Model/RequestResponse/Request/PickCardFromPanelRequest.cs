using System.Collections.Generic;
using Logic.Cards;
using Logic.Model.Player;

namespace Logic.Model.RequestResponse.Request
{
    public class PickCardFromPanelRequest : BaseRequest
    {
        public List<DecorationShowCard> AvailableCards { get; set; }

        /// <summary>
        /// 取牌的最大数量
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// 取牌的最小数量
        /// </summary>
        public int MinCount { get; set; }
    }

    /// <summary>
    /// 卡牌，用来包装用来做显示的牌
    /// </summary>
    public class DecorationShowCard
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        private CardBase _card;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsViewable { get; set; }

        /// <summary>
        /// 被谁选取了
        /// </summary>
        public PlayerHero PickedBy { get; set; }

        public DecorationShowCard(CardBase card)
        {
            this._card = card;
        }
    }
}
