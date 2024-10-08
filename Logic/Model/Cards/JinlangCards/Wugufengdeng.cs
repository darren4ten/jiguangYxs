﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.GameLevel.Panel;
using Logic.Log;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 五谷丰登
    /// </summary>
    public class Wugufengdeng : JinnangBase, IGroupJinnang
    {
        public Wugufengdeng()
        {
            this.Description = "五谷丰登";
            this.Name = "Wugufengdeng";
            this.DisplayName = "五谷丰登";
            this.Image = "/Resources/card/jinnang/card_wugufengdeng.jpg";
        }
        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            cardRequestContext.AttackType = Enums.AttackTypeEnum.Wugufengdeng;
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
            var currentPlayer = PlayerContext.Player;
            var cards = PlayerContext.GameLevel.PickNextCardsFromStack(PlayerContext.GameLevel.Players.Count(p => p.IsAlive)).ToList();
            Console.WriteLine($"五谷丰登可选牌有：【{string.Join("】，", cards)}】。");
            PlayerContext.GameLevel.LogManager.LogAction(
                                    new RichTextParagraph(
                                    new RichTextWrapper("五谷丰登可选牌有"),
                                    new RichTextWrapper(string.Join("】，", cards), RichTextWrapper.GetColor(ColorEnum.Red)),
                                    new RichTextWrapper("。")
                                 ));

            var panel = new PanelBase
            {
                DisplayMessage = "请选择一张要抽取的牌",
                UnknownCards = PanelBase.ConvertToPanelCard(cards, true),
                CardOwner = null,
                IsGlobal = true,
                PanelType = PanelTypeEnum.Wugufengdeng
            };

            //await PlayerContext.Player.PlayerUiState.ShowPanel(panel);
            do
            {
                var req = new CardRequestContext()
                {
                    AttackType = AttackTypeEnum.Wugufengdeng,
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

                var res = await currentPlayer.ResponseCard(new CardRequestContext()
                {
                    MaxCardCountToPlay = 1,
                    MinCardCountToPlay = 1,
                    SrcPlayer = PlayerContext.Player,
                    RequestTaskCompletionSource = new TaskCompletionSource<CardResponseContext>(),
                    TargetPlayers = new List<Player.Player>()
                    {
                        currentPlayer
                    },
                    AttackType = AttackTypeEnum.SelectCard,
                    Message = panel.DisplayMessage,
                    Panel = panel
                }, cardResponseContext, roundContext);
                //todo:摸牌动画以及触发摸牌事件
                await currentPlayer.AddCardsInHand(res.Cards);
                currentPlayer = currentPlayer.GetNextPlayer(false);
            } while (currentPlayer != null && currentPlayer != PlayerContext.Player);
            PlayerContext.Player.PlayerUiState.ClosePanel(panel);

            return cardResponseContext;
        }
    }
}
