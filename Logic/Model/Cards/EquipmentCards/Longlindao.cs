using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.GameLevel.Panel;
using Logic.Log;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 龙鳞刀
    /// </summary>
    public class Longlindao : EquipmentBase, IWeapon
    {
        private Guid eventId;
        public Longlindao()
        {
            this.Description = "龙鳞刀";
            this.Name = "Longlindao";
            this.DisplayName = "龙鳞刀";
            this.Image = "/Resources/card/equipment/card_longlindao.jpg";
            BaseAttackFactor.ShaDistance = 2;
        }

        protected override async Task OnEquip()
        {
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance += BaseAttackFactor.ShaDistance - 1;
            //监听杀成功事件，如果杀成功，可以选择弃掉对方的两张牌（任意）
            eventId = Guid.NewGuid();
            PlayerContext.Player.ListenEvent(eventId, Enums.EventTypeEnum.BeforeShaSuccess, (
                async (context, roundContext, responseContext) =>
                {
                    var target = context.TargetPlayers.First();
                    //目标player身上所有的牌数量是不是大于0，只有大于0才可以选择卸牌
                    if (target.Marks.Count(p => p.MarkType == MarkTypeEnum.Card) + target.CardsInHand.Count + target.EquipmentSet.Count > 0)
                    {
                        bool shouldTrigger = await PlayerContext.Player.ActionManager.OnRequestTriggerSkill(SkillTypeEnum.Longlindao, new CardRequestContext()
                        {
                            CardScope = CardScopeEnum.Any,
                            AttackType = AttackTypeEnum.Longlindao,
                            MaxCardCountToPlay = 2,
                            MinCardCountToPlay = 2,
                            SrcPlayer = PlayerContext.Player,
                            TargetPlayers = new List<Player.Player>() { target }
                        });

                        if (shouldTrigger)
                        {
                            var panelRequest = new PickCardFromPanelRequest()
                            {
                                MaxCount = 2,
                                MinCount = 2,
                                RequestId = Guid.NewGuid(),
                                Panel = new PanelBase()
                                {
                                    CardOwner = target,
                                    DisplayMessage = "请选择弃掉目标2张牌",
                                    InHandCards = PanelBase.ConvertToPanelCard(target.CardsInHand, false),
                                    EquipmentCards = PanelBase.ConvertToPanelCard(target.EquipmentSet, true),
                                    MarkCards = PanelBase.ConvertToPanelCard(target.Marks, true),
                                    PlayersToShare = new ObservableCollection<Player.Player>() { PlayerContext.Player }
                                },
                            };
                            var response = await PlayerContext.Player.ActionManager.OnRequestPickCardFromPanel(panelRequest);

                            //从装备栏中移除 
                            var panelCards = panelRequest.Panel.EquipmentCards?.Where(p => p.SelectedBy == PlayerContext.Player).ToList();
                            panelCards?.ForEach(async p =>
                            {
                                Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】从{panelRequest.Panel.CardOwner.PlayerId}的【{panelRequest.Panel.CardOwner.CurrentPlayerHero.Hero.DisplayName}】抽取了{p.Card.DisplayName}");

                                PlayerContext.GameLevel.LogManager.LogAction(
                                 new RichTextParagraph(
                                 new RichTextWrapper($"{PlayerContext.Player.PlayerId}【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue)),
                                 new RichTextWrapper("从"),
                                 new RichTextWrapper($"{panelRequest.Panel.CardOwner.PlayerId}【{panelRequest.Panel.CardOwner.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue)),
                                 new RichTextWrapper("抽取了"),
                                 new RichTextWrapper(ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                 new RichTextWrapper("。")
                              ));

                                PlayerContext.GameLevel.TempCardDesk.Add(p.Card);

                                if (target.EquipmentSet.Any(e => e == p.Card))
                                {
                                    //是装备牌
                                    await target.RemoveEquipment(p.Card, null, null, null);
                                    panelRequest.Panel.EquipmentCards.Remove(p);
                                }
                            });

                            //移除标记
                            panelCards = panelRequest.Panel.MarkCards?.Where(p => p.SelectedBy == PlayerContext.Player).ToList();
                            panelCards?.ForEach(async p =>
                            {
                                Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】从{panelRequest.Panel.CardOwner.PlayerId}的【{panelRequest.Panel.CardOwner.CurrentPlayerHero.Hero.DisplayName}】抽取了{p.Card.DisplayName}");

                                PlayerContext.GameLevel.LogManager.LogAction(
                                 new RichTextParagraph(
                                 new RichTextWrapper($"{PlayerContext.Player.PlayerId}【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue)),
                                 new RichTextWrapper("从"),
                                 new RichTextWrapper($"{panelRequest.Panel.CardOwner.PlayerId}【{panelRequest.Panel.CardOwner.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue)),
                                 new RichTextWrapper("抽取了"),
                                 new RichTextWrapper(ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                 new RichTextWrapper("。")
                              ));

                                PlayerContext.GameLevel.TempCardDesk.Add(p.Card);

                                if (p.Mark != null)
                                {
                                    //是标记
                                    await target.RemoveMark(p.Mark);
                                    panelRequest.Panel.MarkCards.Remove(p);
                                }
                            });

                            //移除手牌
                            panelCards = panelRequest.Panel.InHandCards?.Where(p => p.SelectedBy == PlayerContext.Player).ToList();
                            panelCards?.ForEach(async p =>
                            {
                                Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】从{panelRequest.Panel.CardOwner.PlayerId}的【{panelRequest.Panel.CardOwner.CurrentPlayerHero.Hero.DisplayName}】抽取了{p.Card.DisplayName}");
                                PlayerContext.GameLevel.TempCardDesk.Add(p.Card);

                                //是标记
                                await target.RemoveCardsInHand(new List<CardBase>() { p.Card }, null, null, null);
                                panelRequest.Panel.InHandCards.Remove(p);
                            });
                            responseContext.ResponseResult = ResponseResultEnum.Cancelled;
                        }
                    }
                }));

            await Task.FromResult(0);
        }

        protected override async Task OnUnEquip()
        {
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance -= BaseAttackFactor.ShaDistance - 1;
            PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.BeforeShaSuccess, eventId);
            await Task.FromResult(0);
        }
    }
}
