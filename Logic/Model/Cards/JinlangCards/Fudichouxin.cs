using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Fudichouxin : CardBase
    {
        public Fudichouxin()
        {
            this.Description = "釜底抽薪";
            this.Name = "Fudichouxin";
            this.DisplayName = "釜底抽薪";
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
