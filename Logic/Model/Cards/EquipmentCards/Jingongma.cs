using System;

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
            this.CardType = Logic.Enums.CardTypeEnum.Weapon;
            AttackFactor.TannangDistance = 1;
            AttackFactor.ShaDistance = 1;
        }

        public override bool CanBePlayed()
        {
            return true;
        }
    }
}
