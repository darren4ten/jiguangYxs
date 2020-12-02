using System;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 狼牙棒
    /// </summary>
    public class Langyabang : EquipmentBase, IWeapon
    {
        public Langyabang()
        {
            this.Description = "狼牙棒";
            this.Name = "Langyabang";
            this.DisplayName = "狼牙棒";
            this.CardType = Logic.Enums.CardTypeEnum.Weapon;
            BaseAttackFactor.ShaDistance = 4;
        }

        public override bool CanBePlayed()
        {
            return true;
        }

    }
}
