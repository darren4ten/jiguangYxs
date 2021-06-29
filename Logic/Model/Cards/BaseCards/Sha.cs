using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.BaseCards
{
    /// <summary>
    /// 杀
    /// </summary>
    public class Sha : CardBase, INeedTargets
    {
        public Sha()
        {
            this.Description = "杀";
            this.Name = "Sha";
            this.DisplayName = "杀";
            this.Image = "/Resources/card/card_sha.jpg";
        }

        /// <summary>
        /// 杀是否可以被打出
        /// </summary>
        /// <param name="playerContext"></param>
        /// <returns></returns>
        public static bool CanBePlayed(PlayerContext playerContext)
        {
            //1.主动出杀,检查出杀的次数
            if (playerContext.Player.IsInZhudongMode()
                && (playerContext.Player.RoundContext.AttackDynamicFactor.MaxShaTimes + playerContext.Player.CurrentPlayerHero.BaseAttackFactor.MaxShaTimes) > playerContext.Player.RoundContext.ShaedTimes)
            {
                return true;
            }

            //2. 被动出杀
            if (CanBeidongPlayCard<Sha>(playerContext))
            {
                return true;
            }
            return false;
        }

        public override bool CanBePlayed()
        {
            return CanBePlayed(PlayerContext);
        }

        public SelectedTargetsRequest GetSelectTargetRequest()
        {
            return new SelectedTargetsRequest()
            {
                MinTargetCount = 1,
                MaxTargetCount = 1,
                CardRequest = CardRequestContext.GetBaseCardRequestContext(null),
                RoundContext = PlayerContext.Player.RoundContext,
                TargetType = AttackTypeEnum.Sha
            };
        }

        /// <summary>
        /// 具体杀的逻辑
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected async Task<CardResponseContext> ExecuteAction(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            if (roundContext != null)
            {
                roundContext.ShaedTimes++;
            }
            foreach (var p in cardRequestContext.TargetPlayers)
            {
                var combindeRequest = new CardRequestContext()
                {
                    AttackType = cardRequestContext.AttackType,
                    FlowerKind = cardRequestContext.FlowerKind,
                    AttackDynamicFactor = cardRequestContext.AttackDynamicFactor,
                    SrcPlayer = cardRequestContext.SrcPlayer,
                    TargetPlayers = new List<Player.Player>() { p }
                };
                //当前杀的目标只有一个
                //检查是否杀不可以闪避，如果是，则跳过
                if (cardRequestContext.AttackDynamicFactor != null && (cardRequestContext.AttackDynamicFactor.IsShaNotAvoidable || (roundContext != null && roundContext.AttackDynamicFactor.IsShaNotAvoidable)))
                {
                    cardResponseContext.Cards = null;
                    cardResponseContext.ResponseResult = ResponseResultEnum.Failed;
                    cardResponseContext.Message = "杀不可被闪避";
                    return await LoseLife(combindeRequest, await CheckResponse(cardRequestContext, cardResponseContext, roundContext), roundContext);
                }

                var actResponse = await p.ResponseCard(cardRequestContext, cardResponseContext, roundContext);
                return await LoseLife(combindeRequest, await CheckResponse(cardRequestContext, actResponse, roundContext), roundContext);
            }

            return cardResponseContext;
        }

        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            var phero = PlayerContext.Player.CurrentPlayerHero;
            cardRequestContext.MaxCardCountToPlay += phero.BaseAttackFactor.ShanCountAvoidSha - 1;//出牌数应该是基本加成数加上英雄实际数量-1
            cardRequestContext.MinCardCountToPlay += phero.BaseAttackFactor.ShanCountAvoidSha - 1;
            cardRequestContext.AttackDynamicFactor = cardRequestContext.AttackDynamicFactor ??
                                                     AttackDynamicFactor.GetDefaultDeltaAttackFactor();
            cardRequestContext.AttackType = cardRequestContext.AttackType == AttackTypeEnum.None ? AttackTypeEnum.Sha : cardRequestContext.AttackType;
            cardRequestContext.RequestCard = new Shan();
            await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.BeforeSha, cardRequestContext, cardResponseContext, roundContext);
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
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.BeforeShaSuccess,
                    cardRequestContext, actResponse, roundContext);
                if (actResponse.ResponseResult == ResponseResultEnum.Cancelled)
                {
                    return actResponse;
                }
                var player = cardRequestContext.TargetPlayers.First();
                await player.CurrentPlayerHero.LoseLife(new LoseLifeRequest()
                {
                    CardRequestContext = cardRequestContext,
                    SrcRoundContext = roundContext,
                    CardResponseContext = actResponse,
                    DamageType = DamageTypeEnum.Sha,
                    RequestId = Guid.NewGuid()
                });

                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.ShaSuccess, cardRequestContext,
                    actResponse, roundContext);
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.AfterShaSuccess,
                    cardRequestContext, actResponse, roundContext);
            }
            else
            {
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.BeforeShaFailed,
                    cardRequestContext, actResponse, roundContext);
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.ShaFailed, cardRequestContext,
                    actResponse, roundContext);
                await PlayerContext.Player.TriggerEvent(Enums.EventTypeEnum.AfterShaFailed,
                    cardRequestContext, actResponse, roundContext);
            }

            return actResponse;
        }

        #endregion
    }
}
