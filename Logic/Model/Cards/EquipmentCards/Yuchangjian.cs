using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Enums;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 鱼肠剑
    /// </summary>
    public class Yuchangjian : EquipmentBase, IWeapon
    {
        private Guid eventId;
        public Yuchangjian()
        {
            this.Description = "鱼肠剑";
            this.Name = "Yuchangjian";
            this.DisplayName = "鱼肠剑";
            BaseAttackFactor.ShaDistance = 2;
        }

        protected override async Task OnEquip()
        {
            //增加攻击距离
            PlayerContext.Player.GetCurrentPlayerHero().BaseAttackFactor.ShaDistance +=
                BaseAttackFactor.ShaDistance - 1;
            //监听BeforeSha事件，
            this.eventId = Guid.NewGuid();
            PlayerContext.Player.ListenEvent(eventId, Enums.EventTypeEnum.BeforeSha, (
               async (context, roundContext, responseContext) =>
               {
                   var target = context.TargetPlayers.FirstOrDefault();
                   if (target == null)
                   {
                       throw new Exception("被杀目标不能为空");
                   }

                   context.AttackDynamicFactor.IsShaNotAvoidableByYuruyi = true;
                   await Task.FromResult(0);
               }));
            await Task.FromResult(0);
        }

        protected override async Task OnUnEquip()
        {
            //扣除攻击距离
            PlayerContext.Player.GetCurrentPlayerHero().BaseAttackFactor.ShaDistance -=
                BaseAttackFactor.ShaDistance - 1;
            //注销监听事件
            PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.AfterShaFailed, eventId);
            await Task.FromResult(0);
        }
    }
}
