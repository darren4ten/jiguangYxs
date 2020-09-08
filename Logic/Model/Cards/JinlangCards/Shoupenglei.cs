using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Shoupenglei : CardBase
    {
        public Shoupenglei()
        {
            this.Description = "手捧雷";
            this.Name = "Shoupenglei";
            this.DisplayName = "手捧雷";
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
