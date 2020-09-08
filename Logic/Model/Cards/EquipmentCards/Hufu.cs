using Logic.Interface;
using Logic.Model.Cards.EquipmentCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Hufu : EquipmentCardBase
    {
        public Hufu()
        {
            this.Description = "虎符";
            this.Name = "Hufu";
            this.DisplayName = "虎符";
            this.CardType = Enums.CardTypeEnum.Weapon;
        }
        public int TannangDistance
        {
            get;
            set;
        } = 0;
        public int AttackDistance
        {
            get;
            set;
        } = 1;
        public int DefenseDistance { get; set; }

        public override bool CanBePlayedFunc()
        {
            return true;
        }

        public override void TriggerResultFunc()
        {
            throw new NotImplementedException();
        }
    }
}
