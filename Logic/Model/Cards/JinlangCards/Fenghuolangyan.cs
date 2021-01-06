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
    /// 烽火狼烟
    /// </summary>
    public class Fenghuolangyan : JinnangBase, IGroupJinnang
    {
        public Fenghuolangyan()
        {
            this.Description = "烽火狼烟";
            this.Name = "Fenghuolangyan";
            this.DisplayName = "烽火狼烟";
        }

        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            cardRequestContext.AttackType = Enums.AttackTypeEnum.Fenghuolangyan;
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
                    AttackType = AttackTypeEnum.Fenghuolangyan,
                    CardScope = CardScopeEnum.Any,
                    AttackDynamicFactor = cardRequestContext.AttackDynamicFactor,
                    RequestCard = new Sha(),
                    MaxCardCountToPlay = 1,
                    MinCardCountToPlay = 1,
                    SrcPlayer = PlayerContext.Player,
                    TargetPlayers = new List<Player.Player>() { currentPlayer }
                };
                //检查是否有无懈可击
                var wxResponse = await GroupRequestWuxiekeji(new CardRequestContext()
                {
                    SrcPlayer = currentPlayer,
                    AttackType = AttackTypeEnum.Fenghuolangyan
                }, cardResponseContext, roundContext);
                if (wxResponse.ResponseResult == Enums.ResponseResultEnum.Wuxiekeji)
                {
                    wxResponse.ResponseResult = Enums.ResponseResultEnum.Success;
                    wxResponse.Message = "请求被无懈可击";
                    currentPlayer = currentPlayer.GetNextPlayer(false);
                    continue;
                }
                var res = await currentPlayer.ResponseCard(req, cardResponseContext, roundContext);
                //判断是否有成功出杀，如果没有，则掉血
                if (res.ResponseResult == ResponseResultEnum.Failed || !res.Cards.Any())
                {
                    await currentPlayer.GetCurrentPlayerHero().LoseLife(new LoseLifeRequest()
                    {
                        CardRequestContext = req,
                        CardResponseContext = res,
                        DamageType = DamageTypeEnum.Fenghuolangyan,
                        SrcRoundContext = roundContext
                    });
                }

                currentPlayer = currentPlayer.GetNextPlayer(false);
            } while (currentPlayer != null && currentPlayer != PlayerContext.Player);

            return cardResponseContext;
        }
    }
}
