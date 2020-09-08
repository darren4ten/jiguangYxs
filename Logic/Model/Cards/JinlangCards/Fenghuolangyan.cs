using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Fenghuolangyan : CardBase
    {
        public Fenghuolangyan()
        {
            this.Description = "烽火狼烟";
            this.Name = "Fenghuolangyan";
            this.DisplayName = "烽火狼烟";
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
