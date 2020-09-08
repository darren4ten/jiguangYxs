using Logic.Interface;
using Logic.Model.Cards.EquipmentCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Luyeqiang : EquipmentCardBase
    {
        public Luyeqiang()
        {
            this.Description = "芦叶枪";
            this.Name = "Luyeqiang";
            this.DisplayName = "芦叶枪";
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
