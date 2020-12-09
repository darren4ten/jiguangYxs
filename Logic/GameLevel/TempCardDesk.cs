using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.Cards;

namespace Logic.GameLevel
{
    /// <summary>
    /// 临时放置卡牌的容器
    /// </summary>
    public class TempCardDesk
    {
        /// <summary>
        /// 所有的卡牌，如果有变形牌，则保存为变牌
        /// </summary>
        public List<CardBase> Cards { get; private set; } = new List<CardBase>();

        public void Add(CardBase card)
        {
            if (card != null)
            {
                card.AttachPlayerContext(null);
                Cards.Add(card);
            }
        }
        /// <summary>
        /// 将牌置于临时牌堆，此举会清理牌堆的关联对象
        /// </summary>
        /// <param name="cards"></param>
        public void Add(List<CardBase> cards)
        {
            foreach (var p in cards)
            {
                Add(p);
            }
        }
    }
}
