using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Jiedaosharen : CardBase
    {
        public Jiedaosharen()
        {
            this.Description = "借刀杀人";
            this.Name = "Jiedaosharen";
            this.DisplayName = "借刀杀人";
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
