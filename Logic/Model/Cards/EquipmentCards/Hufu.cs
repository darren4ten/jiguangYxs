using System;
using System.Threading.Tasks;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 虎符
    /// </summary>
    public class Hufu : EquipmentBase, IWeapon
    {
        public Hufu()
        {
            this.Description = "虎符";
            this.Name = "Hufu";
            this.DisplayName = "虎符";
            BaseAttackFactor.ShaDistance = 1;
            BaseAttackFactor.MaxShaTimes = 9999;
        }
        protected override async Task OnEquip()
        {
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance += BaseAttackFactor.ShaDistance - 1;
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.MaxShaTimes += BaseAttackFactor.MaxShaTimes;
            await Task.FromResult(0);
        }

        protected override async Task OnUnEquip()
        {
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance -= BaseAttackFactor.ShaDistance - 1;
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.MaxShaTimes -= BaseAttackFactor.MaxShaTimes;
            await Task.FromResult(0);
        }
    }
}
