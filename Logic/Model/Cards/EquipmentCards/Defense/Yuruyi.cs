using Logic.Interface;
using Logic.Model.Cards.EquipmentCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards.EquipmentCards
{
    public class Yuruyi : EquipmentCardBase
    {
        public Yuruyi()
        {
            this.Description = "玉如意";
            this.Name = "Yuruyi";
            this.DisplayName = "玉如意";
            this.CardType = Enums.CardTypeEnum.Defender;
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
