using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Event;
using Logic.GameLevel;
using Logic.GameLevel.Panel;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 霸王弓
    /// </summary>
    public class Bawanggong : EquipmentBase, IWeapon
    {
        private Guid eventId;
        public Bawanggong()
        {
            this.Description = "霸王弓";
            this.Name = "Bawanggong";
            this.DisplayName = "霸王弓";

            BaseAttackFactor.TannangDistance = 0;
            BaseAttackFactor.ShaDistance = 5;
        }

        protected override async Task OnEquip()
        {
            //增加攻击距离
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance +=
                BaseAttackFactor.ShaDistance - 1;
            await Task.FromResult(0);
            //监听杀成功事件
            this.eventId = Guid.NewGuid();
            PlayerContext.Player.ListenEvent(eventId, Enums.EventTypeEnum.AfterShaSuccess, (
               async (context, roundContext, responseContext) =>
                {
                    //如果杀成功，且目标有装备进攻马或者防御马，则提示用户是否选择卸掉
                    var target = context.TargetPlayers.FirstOrDefault();
                    if (target == null)
                    {
                        throw new Exception("被杀目标不能为空");
                    }

                    var mas = target.EquipmentSet.Where(p => p is Jingongma || p is Fangyuma);
                    if (mas.Any())
                    {
                        var cards = new List<CardBase>();
                        cards.AddRange(mas);
                        var panelRequest = new PickCardFromPanelRequest()
                        {
                            MaxCount = 1,
                            MinCount = 1,
                            RequestId = Guid.NewGuid(),
                            Panel = new PanelBase()
                            {
                                CardOwner = target,
                                DisplayMessage = "请选择弃掉目标一张马",
                                EquipmentCards = PanelBase.ConvertToPanelCard(cards.ToList(), true),
                                PlayersToShare = new ObservableCollection<Player.Player>() { PlayerContext.Player }
                            },
                        };
                        var response = await PlayerContext.Player.ActionManager.OnRequestPickCardFromPanel(panelRequest);

                        //从装备栏中移除 
                        var panelCards = panelRequest.Panel.EquipmentCards?.Where(p => p.SelectedBy == PlayerContext.Player).ToList();
                        panelCards?.ForEach(async p =>
                        {
                            Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】从{panelRequest.Panel.CardOwner.PlayerId}的【{panelRequest.Panel.CardOwner.CurrentPlayerHero.Hero.DisplayName}】抽取了{p.Card.DisplayName}");
                            await panelRequest.Panel.CardOwner.RemoveEquipment(p.Card, context, response, roundContext);
                            panelRequest.Panel.EquipmentCards.Remove(p);
                            PlayerContext.GameLevel.TempCardDesk.Add(p.Card);
                        });
                    }
                }));
        }

        protected override async Task OnUnEquip()
        {
            //扣除攻击距离
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance -=
                BaseAttackFactor.ShaDistance - 1;
            await Task.FromResult(0);
            //注销监听事件
            PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.AfterShaSuccess, eventId);
        }
    }
}
