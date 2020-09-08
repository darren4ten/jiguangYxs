using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Huadiweilao : CardBase
    {
        public Huadiweilao()
        {
            this.Description = "画地为牢";
            this.Name = "Huadiweilao";
            this.DisplayName = "画地为牢";
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
