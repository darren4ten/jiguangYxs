using Logic.Interface;
using Logic.Model.Cards.EquipmentCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Longlindao : EquipmentCardBase
    {
        public Longlindao()
        {
            this.Description = "龙鳞刀";
            this.Name = "Longlindao";
            this.DisplayName = "龙鳞刀";
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
