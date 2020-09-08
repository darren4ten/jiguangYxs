using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Xiuyangshengxi : CardBase
    {
        public Xiuyangshengxi()
        {
            this.Description = "休养生息";
            this.Name = "Xiuyangshengxi";
            this.DisplayName = "休养生息";
            this.CardType = Enums.CardTypeEnum.Jinlang;
        }

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
