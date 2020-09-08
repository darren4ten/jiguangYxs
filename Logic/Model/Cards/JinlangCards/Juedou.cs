using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Juedou : CardBase
    {
        public Juedou()
        {
            this.Description = "决斗";
            this.Name = "Juedou";
            this.DisplayName = "决斗";
            this.CardType = Enums.CardTypeEnum.Jinlang;
        }

        public override bool CanBePlayedFunc()
        {
            return false;
        }

        public override void TriggerResultFunc()
        {
            throw new NotImplementedException();
        }
    }
}
