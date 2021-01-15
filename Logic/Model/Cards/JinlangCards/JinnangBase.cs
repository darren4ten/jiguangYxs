using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;

namespace Logic.Model.Cards.JinlangCards
{
    public abstract class JinnangBase : CardBase
    {
        public JinnangBase() : base()
        {

        }

        public override bool CanBePlayed()
        {
            return PlayerContext.Player.IsInBeidongMode() || PlayerContext.Player.IsInZhudongMode();
        }

        public override async Task<CardResponseContext> PlayCard(CardRequestContext cardRequestContext,
            RoundContext roundContext)
        {
            cardRequestContext.AttackDynamicFactor = cardRequestContext.AttackDynamicFactor ??
                                                     AttackDynamicFactor.GetDefaultBaseAttackFactor();
            //默认SrcPlayer为当前出牌的人
            cardRequestContext.SrcPlayer = cardRequestContext.SrcPlayer ?? PlayerContext.Player;
            Console.WriteLine($"[{cardRequestContext.SrcPlayer.PlayerName}{cardRequestContext.SrcPlayer.PlayerId}的【{cardRequestContext.SrcPlayer.CurrentPlayerHero.Hero.DisplayName}】]{(cardRequestContext.TargetPlayers?.Any() == true ? "向" + string.Join(",", cardRequestContext.TargetPlayers.Select(p => p.PlayerName + p.PlayerId)) : "")}打出“{this.DisplayName}”");

            CardResponseContext responseContext = new CardResponseContext();

            await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforeZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            if (!(this is IDelayJinnang))
            {
                await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforePlayJinnang, cardRequestContext, responseContext, roundContext);
            }

            if (responseContext.ResponseResult != ResponseResultEnum.UnKnown) return responseContext;

            var r1 = await OnBeforePlayCard(cardRequestContext, responseContext, roundContext);
            if (r1.ResponseResult != ResponseResultEnum.UnKnown) return r1;

            await PlayerContext.Player.TriggerEvent(EventTypeEnum.ZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            if (!(this is IDelayJinnang))
            {
                await PlayerContext.Player.TriggerEvent(EventTypeEnum.PlayJinnang, cardRequestContext, responseContext,
                    roundContext);
                //非延时锦囊，将该牌置入TempCardDesk
                await PlayerContext.Player.RemoveCardsInHand(new List<CardBase>() { this }, cardRequestContext,
                    responseContext, roundContext);
            }
            else
            {
                //延时锦囊，只从手中移除
                await PlayerContext.Player.RemoveCardsButNotThrow(PlayerContext.Player, new List<CardBase>() { this });
            }

            //检查是否有无懈可击
            if (!(this is IDelayJinnang || this is IGroupJinnang))
            {
                var wxResponse = await GroupRequestWuxiekeji(new CardRequestContext()
                {
                    SrcPlayer = cardRequestContext.TargetPlayers?.FirstOrDefault() ?? cardRequestContext.SrcPlayer,
                    AttackType = cardRequestContext.AttackType,
                    SrcCards = cardRequestContext.SrcCards,
                    AttackDynamicFactor = cardRequestContext.AttackDynamicFactor
                }, responseContext, roundContext);
                if (wxResponse.ResponseResult == Enums.ResponseResultEnum.Wuxiekeji)
                {
                    wxResponse.ResponseResult = Enums.ResponseResultEnum.Wuxiekeji;
                    wxResponse.Message = "请求被无懈可击";
                    return wxResponse;
                }
            }

            var r2 = await OnPlayCard(cardRequestContext, r1, roundContext);


            var r3 = await OnAfterPlayCard(cardRequestContext, r2, roundContext);
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            if (!(this is IDelayJinnang))
            {
                await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterPlayJinnang, cardRequestContext,
                    responseContext, roundContext);
            }

            return r3;
        }

        /// <summary>
        /// 询问无懈可击
        /// </summary>
        /// <returns></returns>
        public async Task<CardResponseContext> GroupRequestWuxiekeji(CardRequestContext request, CardResponseContext response, RoundContext roundContext)
        {
            return await PlayerContext.GameLevel.GroupRequestWithConfirm(new CardRequestContext()
            {
                RequestCard = new Wuxiekeji(),
                SrcPlayer = request.SrcPlayer,
                MinCardCountToPlay = 1,
                MaxCardCountToPlay = 1,
                AttackType = request.AttackType,
                TargetPlayers = PlayerContext.GameLevel.Players.Where(p => p.IsAlive).ToList()
            });
        }
    }
}
