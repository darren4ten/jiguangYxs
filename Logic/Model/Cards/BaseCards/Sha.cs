using System;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.BaseCards
{
    /// <summary>
    /// 杀
    /// </summary>
    public class Sha : CardBase
    {
        public Sha()
        {
            this.Description = "杀";
            this.Name = "Sha";
            this.DisplayName = "杀";
        }

        public override bool CanBePlayed()
        {
            //1.主动出杀,检查出杀的次数
            if (PlayerContext.Player.IsInZhudongMode()
                && PlayerContext.Player.RoundContext.AttackDynamicFactor.MaxShaTimes > PlayerContext.Player.RoundContext.ShaedTimes)
            {
                return true;
            }

            //2. 被动出杀
            if (PlayerContext.Player.IsInBeidongMode())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 具体杀的逻辑
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected async Task<CardResponseContext> ExecuteAction(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {

            PlayerContext.Player.RoundContext.ShaedTimes++;
            foreach (var p in cardRequestContext.TargetPlayers)
            {
                //检查是否杀不可以闪避，如果是，则跳过
                if (cardRequestContext.AttackDynamicFactor.IsShaNotAvoidable || roundContext.AttackDynamicFactor.IsShaNotAvoidable)
                {
                    cardResponseContext.Cards = null;
                    cardResponseContext.ResponseResult = ResponseResultEnum.Failed;
                    cardResponseContext.Message = "杀不可被闪避";
                    return await LoseLife(cardRequestContext, await CheckResponse(cardRequestContext, cardResponseContext, roundContext), roundContext);
                }

                var actResponse = await p.ResponseCard(cardRequestContext);
                return await LoseLife(cardRequestContext, await CheckResponse(cardRequestContext, actResponse, roundContext), roundContext);
            }

            return cardResponseContext;
        }

        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.BeforeSha, cardRequestContext, cardResponseContext);
            return cardResponseContext;
        }

        protected override async Task<CardResponseContext> OnPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.Sha, cardRequestContext, cardResponseContext);
            if (PlayerContext.Player.IsInZhudongMode())
            {
                PlayerContext.Player.RoundContext.ShaedTimes++;
            }

            cardRequestContext = cardRequestContext ?? new CardRequestContext()
            {
                AttackDynamicFactor = AttackDynamicFactor.GetDefaultDeltaAttackFactor()
            };

            var res = await ExecuteAction(cardRequestContext, cardResponseContext, roundContext);
            return res;
        }

        protected override async Task<CardResponseContext> OnAfterPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.AfterSha, cardRequestContext, cardResponseContext);

            return cardResponseContext;
        }

        public override async Task Popup()
        {
            if (CanBePlayed())
            {
                await SelectTargets();
            }
        }

        #region Private

        private async Task<CardResponseContext> LoseLife(CardRequestContext cardRequestContext, CardResponseContext actResponse, RoundContext roundContext)
        {
            if (actResponse == null)
            {
                throw new Exception("actResponse cannot be null.");
            }

            //如果返回失败，则掉血
            if (actResponse.ResponseResult == ResponseResultEnum.Failed)
            {
                var player = cardRequestContext.TargetPlayers.First();
                await player.GetCurrentPlayerHero().LoseLife(new LoseLifeRequest()
                {
                    CardRequestContext = cardRequestContext,
                    SrcRoundContext = roundContext,
                    CardResponseContext = actResponse,
                    DamageType = DamageTypeEnum.Sha,
                    RequestId = Guid.NewGuid()
                });
            }

            return actResponse;
        }


        private async Task<CardResponseContext> CheckResponse(CardRequestContext cardRequestContext, CardResponseContext actResponse, RoundContext roundContext)
        {
            if (actResponse == null)
            {
                return new CardResponseContext()
                {
                    ResponseResult = ResponseResultEnum.Failed
                };
            }

            if (actResponse.ResponseResult != ResponseResultEnum.UnKnown)
            {
                return actResponse;
            }

            if (actResponse.Cards != null && actResponse.Cards.Count >= cardRequestContext.MinCardCountToPlay &&
                actResponse.Cards.Count <= cardRequestContext.MaxCardCountToPlay)
            {
                actResponse.ResponseResult = ResponseResultEnum.Success;
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.BeforeShaSuccess,
                    cardRequestContext, actResponse, roundContext);
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.ShaSuccess, cardRequestContext,
                     actResponse, roundContext);
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.AfterShaSuccess,
                    cardRequestContext, actResponse, roundContext);
                return actResponse;
            }
            else
            {
                actResponse.ResponseResult = ResponseResultEnum.Failed;
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.BeforeShaFailed,
                    cardRequestContext, actResponse, roundContext);
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.ShaFailed, cardRequestContext,
                    actResponse, roundContext);
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.AfterShaFailed,
                    cardRequestContext, actResponse, roundContext);
                return actResponse;
            }
        }

        #endregion
    }
}
