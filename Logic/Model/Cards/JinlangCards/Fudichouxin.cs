﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.GameLevel.Panel;
using Logic.Log;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 釜底抽薪
    /// </summary>
    public class Fudichouxin : JinnangBase, INeedTargets
    {
        public Fudichouxin()
        {
            this.Description = "釜底抽薪";
            this.Name = "Fudichouxin";
            this.DisplayName = "釜底抽薪";
            this.Image = "/Resources/card/jinnang/card_fudichouxin.jpg";
        }
        public SelectedTargetsRequest GetSelectTargetRequest()
        {
            return new SelectedTargetsRequest()
            {
                MinTargetCount = 1,
                MaxTargetCount = 1,
                CardRequest = CardRequestContext.GetBaseCardRequestContext(null),
                RoundContext = PlayerContext.Player.RoundContext,
                TargetType = AttackTypeEnum.Fudichouxin
            };
        }

        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            cardRequestContext.AttackType = Enums.AttackTypeEnum.Fudichouxin;
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
            var target = cardRequestContext.TargetPlayers.First();
            //获取target的所有牌，展示面板
            var panel = new PanelBase
            {
                DisplayMessage = "请选择一张要弃掉的牌",
                EquipmentCards = PanelBase.ConvertToPanelCard(target.EquipmentSet, true),
                InHandCards = PanelBase.ConvertToPanelCard(target.CardsInHand, false),//todo:混乱手牌顺序
                MarkCards = PanelBase.ConvertToPanelCard(target.Marks, true),
                CardOwner = target
            };
            var res = await PlayerContext.Player.ResponseCard(new CardRequestContext()
            {
                MaxCardCountToPlay = 1,
                MinCardCountToPlay = 1,
                SrcPlayer = target,
                AttackType = AttackTypeEnum.SelectCard,
                Message = panel.DisplayMessage,
                Panel = panel
            }, cardResponseContext, roundContext);
            //该牌状态改为可以查看
            res.Cards?.ForEach(c => c.IsViewableForOthers = true);
            //移除Panel中SelectedBy为当前player的牌
            await RemoveCardsFromPanel(panel, cardRequestContext, cardResponseContext, roundContext);
            PlayerContext.Player.PlayerUiState.ClosePanel(panel);
            return res;
        }

        protected async Task RemoveCardsFromPanel(PanelBase panel, CardRequestContext request, CardResponseContext response, RoundContext roundContext)
        {
            //检查标记
            var panelCards = panel.MarkCards?.Where(p => p.SelectedBy == PlayerContext.Player || p.Card.IsPopout).ToList();
            panelCards?.ForEach(async p =>
            {
                Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】从{panel.CardOwner.PlayerId}的【{panel.CardOwner.CurrentPlayerHero.Hero.DisplayName}】抽取了{p.Card.DisplayName}");
                PlayerContext.GameLevel.LogManager.LogAction(
                                 new RichTextParagraph(
                                 new RichTextWrapper(PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                 new RichTextWrapper("从"),
                                 new RichTextWrapper(panel.CardOwner.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                 new RichTextWrapper("抽取了标记"),
                                 new RichTextWrapper(p.Card.ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                 new RichTextWrapper("。")
                              ));

                await panel.CardOwner.RemoveMark(p.Mark);
                PlayerContext.GameLevel.TempCardDesk.Add(p.Card);
            });
            //检查装备
            panelCards = panel.EquipmentCards?.Where(p => p.SelectedBy == PlayerContext.Player || p.Card.IsPopout).ToList();
            panelCards?.ForEach(async p =>
            {
                Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】从{panel.CardOwner.PlayerId}的【{panel.CardOwner.CurrentPlayerHero.Hero.DisplayName}】抽取了{p.Card.DisplayName}");
                PlayerContext.GameLevel.LogManager.LogAction(
                                 new RichTextParagraph(
                                 new RichTextWrapper(PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                 new RichTextWrapper("从"),
                                 new RichTextWrapper(panel.CardOwner.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                 new RichTextWrapper("抽取了装备"),
                                 new RichTextWrapper(p.Card.ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                 new RichTextWrapper("。")
                              ));

                await panel.CardOwner.RemoveEquipment(p.Card, request, response, roundContext);
                panel.EquipmentCards.Remove(p);
                PlayerContext.GameLevel.TempCardDesk.Add(p.Card);
            });
            //检查手牌
            panelCards = panel.InHandCards?.Where(p => p.SelectedBy == PlayerContext.Player || p.Card.IsPopout).ToList();
            panelCards?.ForEach(async p =>
            {
                Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】从{panel.CardOwner.PlayerId}的【{panel.CardOwner.CurrentPlayerHero.Hero.DisplayName}】抽取了{p.Card.DisplayName}");
                PlayerContext.GameLevel.LogManager.LogAction(
                                  new RichTextParagraph(
                                  new RichTextWrapper(PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                  new RichTextWrapper("从"),
                                  new RichTextWrapper(panel.CardOwner.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                  new RichTextWrapper("抽取了手牌"),
                                  new RichTextWrapper(p.Card.ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                  new RichTextWrapper("。")
                               ));

                await panel.CardOwner.RemoveCardsInHand(new List<CardBase>() { p.Card }, request, response, roundContext);
                //将牌置于弃牌堆
                PlayerContext.GameLevel.TempCardDesk.Add(p.Card);
            });
            await Task.FromResult(0);
        }

    }
}
