using System.Collections.Generic;
using System.Linq;
using Logic.Cards;
using Logic.Model.Enums;
using Logic.Model.Mark;
using Logic.Model.Player;

namespace Logic.GameLevel.Panel
{
    /// <summary>
    /// 面板模型
    /// </summary>
    public class PanelBase
    {
        /// <summary>
        /// 面板上固定展示的消息内容
        /// </summary>
        public string DisplayMessage { get; set; }

        /// <summary>
        /// 装备栏
        /// </summary>
        public List<PanelCard> EquipmentCards { get; set; }

        /// <summary>
        /// 标记栏
        /// </summary>
        public List<PanelCard> MarkCards { get; set; }

        /// <summary>
        /// 手牌栏
        /// </summary>
        public List<PanelCard> InHandCards { get; set; }

        /// <summary>
        /// 自定义栏
        /// </summary>
        public List<PanelCard> UnknownCards { get; set; }

        /// <summary>
        /// 卡牌来源
        /// </summary>
        public Player CardOwner { get; set; }

        /// <summary>
        /// 需要选牌的player
        /// </summary>
        public List<Player> PlayersToShare { get; set; }

        /// <summary>
        /// 将CardBase转成PanelCard
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="isViewable"></param>
        /// <returns></returns>
        public static List<PanelCard> ConvertToPanelCard(List<CardBase> cards, bool isViewable)
        {
            if (cards == null)
            {
                return null;
            }

            return cards.Select(p => new PanelCard(p, isViewable)).ToList();
        }

        /// <summary>
        /// 将CardBase转成PanelCard
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="isViewable"></param>
        /// <returns></returns>
        public static List<PanelCard> ConvertToPanelCard(List<MarkBase> marks, bool isViewable)
        {
            if (marks == null)
            {
                return null;
            }

            var validMarks = marks.Where(p => p.MarkType == MarkTypeEnum.Card);
            return validMarks.Select(p => new PanelCard(p.Cards.First(), isViewable)).ToList();
        }
    }
}
