using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 烽火狼烟
    /// </summary>
    public class Fenghuolangyan : JinnangBase
    {
        public Fenghuolangyan()
        {
            this.Description = "烽火狼烟";
            this.Name = "Fenghuolangyan";
            this.DisplayName = "烽火狼烟";
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
