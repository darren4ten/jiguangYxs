using System;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 五谷丰登
    /// </summary>
    public class Wugufengdeng : JinnangBase, IGroupJinnang
    {
        public Wugufengdeng()
        {
            this.Description = "五谷丰登";
            this.Name = "Wugufengdeng";
            this.DisplayName = "五谷丰登";
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
