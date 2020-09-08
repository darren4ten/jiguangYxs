using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Wugufengdeng : CardBase
    {
        public Wugufengdeng()
        {
            this.Description = "五谷丰登";
            this.Name = "Wugufengdeng";
            this.DisplayName = "五谷丰登";
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
