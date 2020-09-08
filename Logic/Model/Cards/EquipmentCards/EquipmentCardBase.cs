using Logic.Cards;
using Logic.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Cards.EquipmentCards
{
    public class EquipmentCardBase : CardBase, IWeapon
    {


        public int DefenseDistance { get; set; }
        public int TannangDistance { get; set; }
        public int AttackDistance { get; set; }
        public int MaxShaCount { get; set; }
        public int MaxShaTargetCount { get; set; }

        public override bool CanBePlayedFunc()
        {
            throw new NotImplementedException();
        }

        public override void TriggerResultFunc()
        {
            throw new NotImplementedException();
        }
    }
}
