using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Enums;

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
            this.CardType = Logic.Enums.CardTypeEnum.Base;
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
            //检查是否杀不可以闪避，如果是，则跳过
            if (cardRequestContext.AttackDynamicFactor.IsShaNotAvoidable || roundContext.AttackDynamicFactor.IsShaNotAvoidable)
            {
                cardResponseContext.Cards = null;
                cardResponseContext.ResponseResult = ResponseResultEnum.Failed;
                cardResponseContext.Message = "杀不可被闪避";
                return cardResponseContext;
            }

            var actResponse = await PlayerContext.Player.ResponseCard(cardRequestContext);

            return actResponse;
        }

        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(Enums.EventTypeEnum.BeforeSha, cardRequestContext, roundContext, cardResponseContext);
            return cardResponseContext;
        }

        protected override async Task<CardResponseContext> OnPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(Enums.EventTypeEnum.Sha, cardRequestContext, roundContext, cardResponseContext);
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
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(Enums.EventTypeEnum.AfterSha, cardRequestContext, roundContext, cardResponseContext);
            //判断是否杀成功
            //如果返回结果不是ResponseResultEnum.UnKnown则直接返回
            if (cardResponseContext == null)
            {
                return new CardResponseContext()
                {
                    ResponseResult = ResponseResultEnum.Failed
                };
            }
            if (cardResponseContext.ResponseResult != ResponseResultEnum.UnKnown)
            {
                return cardResponseContext;
            }

            if (cardResponseContext.Cards != null && cardResponseContext.Cards.Count >= cardRequestContext.MinCardCountToPlay && cardResponseContext.Cards.Count <= cardRequestContext.MaxCardCountToPlay)
            {
                cardResponseContext.ResponseResult = ResponseResultEnum.Success;
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(Enums.EventTypeEnum.BeforeShaSuccess, cardRequestContext, roundContext, cardResponseContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(Enums.EventTypeEnum.ShaSuccess, cardRequestContext, roundContext, cardResponseContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(Enums.EventTypeEnum.AfterShaSuccess, cardRequestContext, roundContext, cardResponseContext);
                return cardResponseContext;
            }
            else
            {
                cardResponseContext.ResponseResult = ResponseResultEnum.Failed;
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(Enums.EventTypeEnum.BeforeShaFailed, cardRequestContext, roundContext, cardResponseContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(Enums.EventTypeEnum.ShaFailed, cardRequestContext, roundContext, cardResponseContext);
                await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(Enums.EventTypeEnum.AfterShaFailed, cardRequestContext, roundContext, cardResponseContext);
                return cardResponseContext;
            }
        }

        public override async Task Popup()
        {
            if (CanBePlayed())
            {
                await SelectTargets();
            }
        }

        #region Private

        private void Assert()
        {

        }

        #endregion
    }
}
