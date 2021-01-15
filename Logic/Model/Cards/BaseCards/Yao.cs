using System;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.BaseCards
{
    /// <summary>
    /// 药
    /// </summary>
    public class Yao : CardBase
    {
        public Yao()
        {
            this.Description = "药";
            this.Name = "Yao";
            this.DisplayName = "药";
            this.Image = "/Resources/card/card_yao.jpg";
        }

        /// <summary>
        /// 是否能出药。
        /// 1. 主动出牌
        ///     1.1 自身血量不满
        /// 2. 被动出牌
        ///     2.1 被动出牌时都可以出
        /// </summary>
        /// <returns></returns>
        public override bool CanBePlayed()
        {
            var curPhero = PlayerContext.Player.CurrentPlayerHero;
            if (PlayerContext.Player.IsInZhudongMode() && curPhero.CurrentLife < curPhero.GetAttackFactor().MaxLife)
            {
                return true;
            }

            if (CanBeidongPlayCard<Yao>(PlayerContext))
            {
                return true;
            }
            return false;
        }

        #region 覆盖父类方法
        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            cardRequestContext.AttackDynamicFactor = cardRequestContext.AttackDynamicFactor ??
                                                     AttackDynamicFactor.GetDefaultDeltaAttackFactor();
            return await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.BeforeEatYao, cardRequestContext, cardResponseContext);
        }

        protected override async Task<CardResponseContext> OnPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            await ExecuteAction(cardRequestContext, cardResponseContext, roundContext);
            return await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.EatYao, cardRequestContext, cardResponseContext, roundContext);
        }

        protected override async Task<CardResponseContext> OnAfterPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            return await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.AfterEatYao, cardRequestContext, cardResponseContext, roundContext);
        }

        #endregion

        #region 业务逻辑

        /// <summary>
        /// 具体吃药的逻辑
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected async Task<CardResponseContext> ExecuteAction(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            var target = cardRequestContext.SrcPlayer ?? PlayerContext.Player;
            await target.CurrentPlayerHero.AddLife(new AddLifeRequest()
            {
                CardRequestContext = cardRequestContext,
                CardResponseContext = cardResponseContext,
                SrcRoundContext = roundContext,
                RecoverType = RecoverTypeEnum.Yao,
                RequestId = Guid.NewGuid()
            });
            return cardResponseContext;
        }

        #endregion
    }
}
