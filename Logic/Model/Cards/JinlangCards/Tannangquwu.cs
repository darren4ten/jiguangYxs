using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Tannangquwu : CardBase
    {
        public Tannangquwu()
        {
            this.Description = "探囊取物";
            this.Name = "Tannangquwu";
            this.DisplayName = "探囊取物";
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
