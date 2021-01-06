using System;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 无中生有
    /// </summary>
    public class Wuzhongshengyou : JinnangBase
    {
        public Wuzhongshengyou()
        {
            this.Description = "无中生有";
            this.Name = "Wuzhongshengyou";
            this.DisplayName = "无中生有";
        }

        protected override async Task<CardResponseContext> OnPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            return await ExecuteAction(cardRequestContext, cardResponseContext, roundContext);
        }

        /// <summary>
        /// 具体的逻辑
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected async Task<CardResponseContext> ExecuteAction(CardRequestContext cardRequestContext,
            CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            var combined = PlayerContext.Player.GetCombindCardRequestContext(cardRequestContext, null, roundContext);
            await PlayerContext.Player.PickCard(combined.AttackDynamicFactor.WuzhongshengyouCardCount);
            return await Task.FromResult(cardResponseContext);
        }
    }
}
