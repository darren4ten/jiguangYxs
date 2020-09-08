using Logic.Interface;
using Logic.Model.Cards.EquipmentCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Bawanggong : EquipmentCardBase
    {
        public Bawanggong()
        {
            this.Description = "霸王弓";
            this.Name = "Bawanggong";
            this.DisplayName = "霸王弓";
            this.CardType = Enums.CardTypeEnum.Weapon;
        }
        public new int TannangDistance
        {
            get;
            set;
        } = 0;
        public new int AttackDistance
        {
            get;
            set;
        } = 5;
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
