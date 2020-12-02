using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 手捧雷
    /// </summary>
    public class Shoupenglei : CardBase
    {
        public Shoupenglei()
        {
            this.Description = "手捧雷";
            this.Name = "Shoupenglei";
            this.DisplayName = "手捧雷";
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
