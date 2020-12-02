using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.MutedCards
{
    /// <summary>
    /// 组合牌
    /// </summary>
    public class CombinedCard : CardBase
    {
        /// <summary>
        /// 原始牌
        /// </summary>
        public List<CardBase> OriginalCards { get; private set; }

        /// <summary>
        /// 目标牌
        /// </summary>
        public CardBase TargetCard { get; private set; }

        public CombinedCard(List<CardBase> originalCards, CardBase targetCard)
        {
            this.OriginalCards = originalCards;
            this.TargetCard = targetCard;
        }

        public override bool CanBePlayed()
        {
            return TargetCard.CanBePlayed();
        }

        public override async Task Popup()
        {
            await TargetCard.Popup();
        }
    }
}
