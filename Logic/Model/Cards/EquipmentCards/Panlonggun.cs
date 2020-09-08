using Logic.Interface;
using Logic.Model.Cards.EquipmentCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Panlonggun : EquipmentCardBase
    {
        public Panlonggun()
        {
            this.Description = "盘龙棍";
            this.Name = "Panlonggun";
            this.DisplayName = "盘龙棍";
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
        } = 3;
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
