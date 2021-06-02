using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.Cards;
using System.Collections.ObjectModel;
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
        public ObservableCollection<CardBase> Cards { get; private set; } = new ObservableCollection<CardBase>();

        public void Clear()
        {
            //恢复卡牌原有状态
            foreach (var card in Cards)
            {
                card.IsViewableForOthers = false;
                card.IsPopout = false;
            }
            Cards.Clear();
        }

        public void Add(CardBase card)
        {
            if (card != null && !Cards.Contains(card))
            {
                //todo:将所有Card的上下文注销掉
                //card.AttachPlayerContext(null);
              
                Cards.Add(card);
            }
        }
        /// <summary>
        /// 将牌置于临时牌堆，此举会清理牌堆的关联对象
        /// </summary>
        /// <param name="cards"></param>
        public void Add(IEnumerable<CardBase> cards)
        {
            foreach (var p in cards)
            {
                Add(p);
            }
        }
    }
}
