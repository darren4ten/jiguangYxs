using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.JinlangCards
{
    public class Wuzhongshengyou : CardBase
    {
        public Wuzhongshengyou()
        {
            this.Description = "无中生有";
            this.Name = "Wuzhongshengyou";
            this.DisplayName = "无中生有";
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
