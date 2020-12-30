using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Enums;

namespace Logic.Model.Cards.MutedCards
{
    /// <summary>
    /// 变换拍
    /// </summary>
    public class ChangedCard : CardBase
    {
        public CardChangeTypeEnum CardChangeType { get; set; }
        /// <summary>
        /// 原始牌
        /// </summary>
        public List<CardBase> OriginalCards { get; private set; }

        /// <summary>
        /// 目标牌
        /// </summary>
        public CardBase TargetCard { get; private set; }

        public ChangedCard(List<CardBase> originalCards, CardBase targetCard)
        {
            this.OriginalCards = originalCards;
            this.TargetCard = targetCard;
            this.DisplayName = targetCard.DisplayName;
            Description = targetCard.Description;
            FlowerKind = targetCard.FlowerKind;
            this.Number = targetCard.Number;
            this.CardId = targetCard.CardId;
            this.Image = targetCard.Image;
        }

        public override bool CanBePlayed()
        {
            return TargetCard.CanBePlayed();
        }

        public override async Task Popup()
        {
            await TargetCard.Popup();
        }

        /// <summary>
        /// 使用TargetCard的PlayCard方法。
        /// TODO：检查OnAfter/OnBefore这些方法是否会都使用target的。
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        public override async Task<CardResponseContext> PlayCard(CardRequestContext cardRequestContext, RoundContext roundContext)
        {
            return await TargetCard.PlayCard(cardRequestContext, roundContext);
        }
    }
}
