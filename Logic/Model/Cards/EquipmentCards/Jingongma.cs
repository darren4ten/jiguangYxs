using Logic.Interface;
using Logic.Model.Cards.EquipmentCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Jingongma : EquipmentCardBase
    {
        public Jingongma()
        {
            this.Description = "进攻马";
            this.Name = "Jingongma";
            this.DisplayName = "进攻马";
            this.CardType = Enums.CardTypeEnum.Weapon;
        }

        public int TannangDistance
        {
            get;
            set;
        } = 1;
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
