using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 手捧雷
    /// </summary>
    public class Shoupenglei : JinnangBase
    {
        public Shoupenglei()
        {
            this.Description = "手捧雷";
            this.Name = "Shoupenglei";
            this.DisplayName = "手捧雷";
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
