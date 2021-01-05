using System;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Enums;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 无懈可击
    /// </summary>
    public class Wuxiekeji : JinnangBase
    {
        public Wuxiekeji()
        {
            this.Description = "无懈可击";
            this.Name = "Wuxiekeji";
            this.DisplayName = "无懈可击";
        }

        public override Task Popup()
        {
            throw new NotImplementedException();
        }

        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            cardRequestContext.AttackType = AttackTypeEnum.Wuxiekeji;
            return await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.BeforeBeRequestedWuxiekeji, cardRequestContext,
                cardResponseContext, roundContext);
        }
    }
}
