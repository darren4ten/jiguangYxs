using Logic.Cards;
using Logic.Model.Mark;
using Logic.Model.Player;

namespace Logic.GameLevel.Panel
{
    /// <summary>
    /// 面板展示的牌
    /// </summary>
    public class PanelCard
    {
        public PanelCard(CardBase card,bool isViewable)
        {
            Card = card;
            card.IsViewableForOthers = isViewable;
        }

        public CardBase Card { get; }

        /// <summary>
        /// 如果该牌是标记，则将标记赋值
        /// </summary>
        public MarkBase Mark { get; set; }

        /// <summary>
        /// 选中该卡牌的人
        /// </summary>
        public Player SelectedBy { get; set; }
    }
}
