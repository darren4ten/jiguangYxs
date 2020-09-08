using Logic.Interface;
using Logic.Model.Cards.EquipmentCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Fangyuma : EquipmentCardBase
    {
        public Fangyuma()
        {
            this.Description = "防御马";
            this.Name = "Fangyuma";
            this.DisplayName = "防御马";
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
        } = 2;
        public int DefenseDistance { get; set; } = 1;

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
