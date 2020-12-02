using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 釜底抽薪
    /// </summary>
    public class Fudichouxin : CardBase
    {
        public Fudichouxin()
        {
            this.Description = "釜底抽薪";
            this.Name = "Fudichouxin";
            this.DisplayName = "釜底抽薪";
            this.CardType = Logic.Enums.CardTypeEnum.Jinlang;
        }

        public override bool CanBePlayed()
        {
            return true;
        }

        public override Task Popup()
        {
            throw new NotImplementedException();
        }
    }
}
