using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 五谷丰登
    /// </summary>
    public class Wugufengdeng : CardBase
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
