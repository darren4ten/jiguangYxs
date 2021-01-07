using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 休养生息
    /// </summary>
    public class Xiuyangshengxi : JinnangBase, IGroupJinnang
    {
        public Xiuyangshengxi()
        {
            this.Description = "休养生息";
            this.Name = "Xiuyangshengxi";
            this.DisplayName = "休养生息";
        }

        public override bool CanBePlayed()
        {
            //必须场上活人有血量不满时才能打出
            return base.CanBePlayed() && PlayerContext.GameLevel.Players.Any(p => p.IsAlive() && p.GetCurrentPlayerHero().CurrentLife < p.GetCurrentPlayerHero().BaseAttackFactor.MaxLife);
        }

        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            cardRequestContext.AttackType = Enums.AttackTypeEnum.Wanjianqifa;
            cardRequestContext.AttackDynamicFactor =
                cardRequestContext.AttackDynamicFactor ?? AttackDynamicFactor.GetDefaultBaseAttackFactor();
            return await Task.FromResult(cardResponseContext);
        }

        protected override async Task<CardResponseContext> OnPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            return await ExecuteAction(cardRequestContext, cardResponseContext, roundContext);
        }

        /// <summary>
        /// 具体的逻辑
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected async Task<CardResponseContext> ExecuteAction(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            var currentPlayer = PlayerContext.Player.GetNextPlayer(false);
            do
            {
                var req = new CardRequestContext()
                {
                    AttackType = AttackTypeEnum.Xiuyangshengxi,
                    CardScope = CardScopeEnum.Any,
                    AttackDynamicFactor = cardRequestContext.AttackDynamicFactor,
                    RequestCard = null,
                    MaxCardCountToPlay = 1,
                    MinCardCountToPlay = 1,
                    SrcPlayer = PlayerContext.Player,
                    TargetPlayers = new List<Player.Player>() { currentPlayer }
                };
                //检查是否有无懈可击
                var wxResponse = await GroupRequestWuxiekeji(req, cardResponseContext, roundContext);
                if (wxResponse.ResponseResult == Enums.ResponseResultEnum.Wuxiekeji)
                {
                    wxResponse.ResponseResult = Enums.ResponseResultEnum.Success;
                    wxResponse.Message = "请求被无懈可击";
                    currentPlayer = currentPlayer.GetNextPlayer(false);
                    continue;
                }

                await currentPlayer.GetCurrentPlayerHero().AddLife(new AddLifeRequest()
                {
                    CardRequestContext = req,
                    CardResponseContext = cardResponseContext,
                    RecoverType = RecoverTypeEnum.Xiuyangshengxi,
                    SrcRoundContext = roundContext
                });

                currentPlayer = currentPlayer.GetNextPlayer(false);
            } while (currentPlayer != null && currentPlayer != PlayerContext.Player);

            return cardResponseContext;
        }
    }
}
