using System;
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
            this.CardType = Logic.Enums.CardTypeEnum.Weapon;
            BaseAttackFactor.ShaDistance = 1;
            BaseAttackFactor.MaxShaTimes = 9999;
        }

        public override bool CanBePlayed()
        {
            return true;
        }
    }
}
