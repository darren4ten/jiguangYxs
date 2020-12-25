using System;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 万箭齐发
    /// </summary>
    public class Wanjianqifa : JinnangBase, IGroupJinnang
    {
        public Wanjianqifa()
        {
            this.Description = "万箭齐发";
            this.Name = "Wanjianqifa";
            this.DisplayName = "万箭齐发";
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
