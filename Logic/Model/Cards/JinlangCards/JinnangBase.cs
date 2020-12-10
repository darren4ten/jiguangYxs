﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
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

        public abstract override Task Popup();

        public override async Task<CardResponseContext> PlayCard(CardRequestContext cardRequestContext,
            RoundContext roundContext)
        {
            //默认SrcPlayer为当前出牌的人
            cardRequestContext.SrcPlayer = cardRequestContext.SrcPlayer ?? PlayerContext.Player;
            Console.WriteLine($"[{cardRequestContext.SrcPlayer.PlayerName}{cardRequestContext.SrcPlayer.PlayerId}的【{cardRequestContext.SrcPlayer.GetCurrentPlayerHero().Hero.DisplayName}】]{(cardRequestContext.TargetPlayers?.Any() == true ? "向" + string.Join(",", cardRequestContext.TargetPlayers.Select(p => p.PlayerName + p.PlayerId)) : "")}打出“{this.DisplayName}”");

            CardResponseContext responseContext = new CardResponseContext();
            //检查是否有无懈可击
            if (!(this is Shoupenglei))
            {
                var wxResponse = await GroupRequestWuxiekeji(cardRequestContext, responseContext, roundContext);
                if (wxResponse.ResponseResult == Enums.ResponseResultEnum.Wuxiekeji)
                {
                    wxResponse.ResponseResult = Enums.ResponseResultEnum.Success;
                    wxResponse.Message = "请求被无懈可击";
                    return wxResponse;
                }
            }

            await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforeZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            if (!(this is Shoupenglei))
            {
                await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforePlayJinnang, cardRequestContext, responseContext, roundContext);
            }

            if (responseContext.ResponseResult != ResponseResultEnum.UnKnown) return responseContext;

            var r1 = await OnBeforePlayCard(cardRequestContext, responseContext, roundContext);
            if (r1.ResponseResult != ResponseResultEnum.UnKnown) return r1;

            await PlayerContext.Player.TriggerEvent(EventTypeEnum.ZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            if (!(this is Shoupenglei))
            {
                await PlayerContext.Player.TriggerEvent(EventTypeEnum.PlayJinnang, cardRequestContext, responseContext,
                    roundContext);
            }

            var r2 = await OnPlayCard(cardRequestContext, r1, roundContext);

            var r3 = await OnAfterPlayCard(cardRequestContext, r2, roundContext);
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            if (!(this is Shoupenglei))
            {
                await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterPlayJinnang, cardRequestContext,
                    responseContext, roundContext);
            }

            //将该牌置入TempCardDesk
            PlayerContext.Player.CardsInHand.Remove(this);
            PlayerContext.GameLevel.TempCardDesk.Add(this);
            return r3;
        }

        /// <summary>
        /// 询问无懈可击
        /// </summary>
        /// <returns></returns>
        protected async Task<CardResponseContext> GroupRequestWuxiekeji(CardRequestContext request, CardResponseContext response, RoundContext roundContext)
        {
            //TODO:循环询问无懈可击
            return await Task.FromResult(response);
        }
    }
}
