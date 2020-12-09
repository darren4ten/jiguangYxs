using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 画地为牢
    /// </summary>
    public class Huadiweilao : JinnangBase
    {
        public Huadiweilao()
        {
            this.Description = "画地为牢";
            this.Name = "Huadiweilao";
            this.DisplayName = "画地为牢";
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
