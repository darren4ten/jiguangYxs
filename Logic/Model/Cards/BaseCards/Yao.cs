using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.BaseCards
{
    public class Yao : CardBase
    {
        public Yao()
        {
            this.Description = "药";
            this.Name = "Yao";
            this.DisplayName = "药";
            this.CardType = Enums.CardTypeEnum.Base;
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
