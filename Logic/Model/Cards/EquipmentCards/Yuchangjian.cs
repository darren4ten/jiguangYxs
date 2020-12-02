using System;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 鱼肠剑
    /// </summary>
    public class Yuchangjian : EquipmentBase, IWeapon
    {
        public Yuchangjian()
        {
            this.Description = "鱼肠剑";
            this.Name = "Yuchangjian";
            this.DisplayName = "鱼肠剑";
            this.CardType = Logic.Enums.CardTypeEnum.Weapon;
            AttackFactor.ShaDistance = 2;
        }

        public override bool CanBePlayed()
        {
            return true;
        }
    }
}
