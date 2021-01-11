using System;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 决斗
    /// </summary>
    public class Juedou : JinnangBase, INeedTargets
    {
        public Juedou()
        {
            this.Description = "决斗";
            this.Name = "Juedou";
            this.DisplayName = "决斗";
        }

        /// <summary>
        /// 决斗。
        /// 1. 主动出牌
        /// 2. 被动出牌.被要求出牌
        /// </summary>
        /// <returns></returns>
        public override bool CanBePlayed()
        {
            return PlayerContext.Player.IsInBeidongMode() || PlayerContext.Player.IsInZhudongMode();
        }

        public SelectedTargetsRequest GetSelectTargetRequest()
        {
            return new SelectedTargetsRequest()
            {
                MinTargetCount = 1,
                MaxTargetCount = 1,
                CardRequest = CardRequestContext.GetBaseCardRequestContext(null),
                RoundContext = PlayerContext.Player.RoundContext,
                TargetType = AttackTypeEnum.Juedou
            };
        }

        #region 覆盖父类方法

        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            var phero = PlayerContext.Player.GetCurrentPlayerHero();
            cardRequestContext.RequestCard = new Sha();
            cardRequestContext.AttackType = AttackTypeEnum.Juedou;
            cardRequestContext.MaxCardCountToPlay += phero.BaseAttackFactor.ShaCountAvoidJuedou;
            cardRequestContext.MinCardCountToPlay += phero.BaseAttackFactor.ShaCountAvoidJuedou;
            return await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.BeforeJuedou, cardRequestContext,
                  cardResponseContext, roundContext);
        }

        protected override async Task<CardResponseContext> OnPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.Juedou, cardRequestContext,
               cardResponseContext, roundContext);
            cardRequestContext.AttackDynamicFactor = cardRequestContext.AttackDynamicFactor ??
                                                     AttackDynamicFactor.GetDefaultDeltaAttackFactor();
            var actResponse = await ExecuteAction(cardRequestContext, cardResponseContext, roundContext);
            return await LoseLife(cardRequestContext, await CheckResponse(cardRequestContext, actResponse, roundContext), roundContext);
        }

        protected override async Task<CardResponseContext> OnAfterPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            return await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.AfterJuedou, cardRequestContext, cardResponseContext, roundContext);
        }

        #endregion

        #region 业务逻辑

        /// <summary>
        /// 具体决斗的逻辑
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected async Task<CardResponseContext> ExecuteAction(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            var target = cardRequestContext.TargetPlayers.First();
            var resTarget = await target.ResponseCard(cardRequestContext, cardResponseContext, roundContext);
            bool isSuccess = true;
            while (resTarget.ResponseResult == Enums.ResponseResultEnum.Success)
            {
                isSuccess = false;
                //被决斗方有可能要求出多张杀
                var targetHero = target.GetCurrentPlayerHero();
                cardRequestContext.MaxCardCountToPlay = targetHero.BaseAttackFactor.ShaCountAvoidJuedou;
                cardRequestContext.MinCardCountToPlay = targetHero.BaseAttackFactor.ShaCountAvoidJuedou;
                resTarget = await PlayerContext.Player.ResponseCard(cardRequestContext, cardResponseContext, roundContext);
                if (resTarget.ResponseResult == ResponseResultEnum.Success)
                {
                    isSuccess = true;
                    resTarget = await target.ResponseCard(cardRequestContext, cardResponseContext, roundContext);
                }
            }
            //如果isSuccess=true则代表当前玩家决斗成功，敌方没有闪避成功
            resTarget.ResponseResult = isSuccess ? ResponseResultEnum.Failed : ResponseResultEnum.Success;
            return resTarget;
        }


        private async Task<CardResponseContext> LoseLife(CardRequestContext cardRequestContext, CardResponseContext actResponse, RoundContext roundContext)
        {
            if (actResponse == null)
            {
                throw new Exception("actResponse cannot be null.");
            }

            //如果返回失败，则掉血
            if (actResponse.ResponseResult == ResponseResultEnum.Failed)
            {
                var target = cardRequestContext.TargetPlayers.First();
                await target.TriggerEvent(Enums.EventTypeEnum.BeforeJuedouSuccess,
                    cardRequestContext, actResponse, roundContext);
                await target.GetCurrentPlayerHero().LoseLife(new LoseLifeRequest()
                {
                    CardRequestContext = cardRequestContext,
                    SrcRoundContext = roundContext,
                    CardResponseContext = actResponse,
                    DamageType = DamageTypeEnum.Juedou,
                    RequestId = Guid.NewGuid()
                });

                await target.TriggerEvent(Enums.EventTypeEnum.JuedouSuccess, cardRequestContext,
                    actResponse, roundContext);
                await target.TriggerEvent(Enums.EventTypeEnum.AfterJuedouSuccess,
                    cardRequestContext, actResponse, roundContext);
            }
            else
            {
                //否则则是决斗失败，自己掉血

                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.BeforeJuedouFailed,
                    cardRequestContext, actResponse, roundContext);
                await PlayerContext.Player.GetCurrentPlayerHero().LoseLife(new LoseLifeRequest()
                {
                    CardRequestContext = cardRequestContext,
                    SrcRoundContext = roundContext,
                    CardResponseContext = actResponse,
                    DamageType = DamageTypeEnum.Juedou,
                    RequestId = Guid.NewGuid()
                });
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.JuedouFailed, cardRequestContext,
                    actResponse, roundContext);
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.AfterJuedouFailed,
                    cardRequestContext, actResponse, roundContext);
            }

            return actResponse;
        }

        #endregion
    }
}
