using System;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 芦叶枪
    /// </summary>
    public class Luyeqiang : EquipmentBase, IWeapon
    {
        public Luyeqiang()
        {
            this.Description = "芦叶枪";
            this.Name = "Luyeqiang";
            this.DisplayName = "芦叶枪";
            this.CardType = Logic.Enums.CardTypeEnum.Weapon;
            BaseAttackFactor.ShaDistance = 3;
        }

        public override bool CanBePlayed()
        {
            return true;
        }

        public new bool IsViewableInSkillPanel()
        {
            return true;
        }
    }
}
