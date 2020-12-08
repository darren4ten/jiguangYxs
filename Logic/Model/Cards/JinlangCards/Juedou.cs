using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 决斗
    /// </summary>
    public class Juedou : CardBase
    {
        public Juedou()
        {
            this.Description = "决斗";
            this.Name = "Juedou";
            this.DisplayName = "决斗";
        }

        public override bool CanBePlayed()
        {
            return false;
        }

        public override Task Popup()
        {
            throw new NotImplementedException();
        }
    }
}
