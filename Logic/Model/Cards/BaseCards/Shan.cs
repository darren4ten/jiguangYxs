using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.BaseCards
{
    public class Shan : CardBase
    {
        public Shan()
        {
            this.Description = "闪";
            this.Name = "Shan";
            this.DisplayName = "闪";
            this.CardType = Enums.CardTypeEnum.Base;
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
