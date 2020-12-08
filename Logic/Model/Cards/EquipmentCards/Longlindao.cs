using System;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 龙鳞刀
    /// </summary>
    public class Longlindao : EquipmentBase, IWeapon
    {
        public Longlindao()
        {
            this.Description = "龙鳞刀";
            this.Name = "Longlindao";
            this.DisplayName = "龙鳞刀";
            BaseAttackFactor.ShaDistance = 2;
        }

        public override bool CanBePlayed()
        {
            return true;
        }
    }
}
