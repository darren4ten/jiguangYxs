using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Wanjianqifa : CardBase
    {
        public Wanjianqifa()
        {
            this.Description = "万箭齐发";
            this.Name = "Wanjianqifa";
            this.DisplayName = "万箭齐发";
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
