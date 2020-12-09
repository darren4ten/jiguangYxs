using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 借刀杀人
    /// </summary>
    public class Jiedaosharen : JinnangBase
    {
        public Jiedaosharen()
        {
            this.Description = "借刀杀人";
            this.Name = "Jiedaosharen";
            this.DisplayName = "借刀杀人";
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
