using System;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 盘龙棍
    /// </summary>
    public class Panlonggun : EquipmentBase, IWeapon
    {
        public Panlonggun()
        {
            this.Description = "盘龙棍";
            this.Name = "Panlonggun";
            this.DisplayName = "盘龙棍";
            BaseAttackFactor.ShaDistance = 3;
        }

        public override bool CanBePlayed()
        {
            return true;
        }
    }
}
