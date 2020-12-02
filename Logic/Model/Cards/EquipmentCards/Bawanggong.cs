using System;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 霸王弓
    /// </summary>
    public class Bawanggong : EquipmentBase, IWeapon
    {
        public Bawanggong()
        {
            this.Description = "霸王弓";
            this.Name = "Bawanggong";
            this.DisplayName = "霸王弓";
            this.CardType = Logic.Enums.CardTypeEnum.Weapon;

            BaseAttackFactor.TannangDistance = 0;
            BaseAttackFactor.ShaDistance = 5;
        }

        public override bool CanBePlayed()
        {
            return true;
        }
    }
}
