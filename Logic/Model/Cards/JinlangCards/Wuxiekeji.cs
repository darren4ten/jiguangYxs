using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Wuxiekeji : CardBase
    {
        public Wuxiekeji()
        {
            this.Description = "无懈可击";
            this.Name = "Wuxiekeji";
            this.DisplayName = "无懈可击";
            this.CardType = Enums.CardTypeEnum.Jinlang;
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
