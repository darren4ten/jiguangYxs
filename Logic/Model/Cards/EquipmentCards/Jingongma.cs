using System;
using System.Threading.Tasks;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 进攻马
    /// </summary>
    public class Jingongma : EquipmentBase
    {
        public Jingongma()
        {
            this.Description = "进攻马";
            this.Name = "Jingongma";
            this.DisplayName = "进攻马";
            BaseAttackFactor.TannangDistance = 1;
            BaseAttackFactor.ShaDistance = 1;
        }

        protected override async Task OnEquip()
        {
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.TannangDistance += BaseAttackFactor.TannangDistance;
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance += BaseAttackFactor.ShaDistance;
            await Task.FromResult(0);
        }

        protected override async Task OnUnEquip()
        {
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.TannangDistance -= BaseAttackFactor.TannangDistance;
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance -= BaseAttackFactor.ShaDistance;
            await Task.FromResult(0);
        }
    }
}
