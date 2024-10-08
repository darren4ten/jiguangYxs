﻿using System;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Enums;

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
            this.Image = "/Resources/card/jinnang/card_wuzhongshengyou.jpg";
        }

        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            cardRequestContext.AttackType = AttackTypeEnum.Wuzhongshengyou;
            return await base.OnBeforePlayCard(cardRequestContext, cardResponseContext, roundContext);
        }

        protected override async Task<CardResponseContext> OnAfterPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
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
