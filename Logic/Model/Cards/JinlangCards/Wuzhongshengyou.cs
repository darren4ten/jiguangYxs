using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 无中生有
    /// </summary>
    public class Wuzhongshengyou : CardBase
    {
        public Wuzhongshengyou()
        {
            this.Description = "无中生有";
            this.Name = "Wuzhongshengyou";
            this.DisplayName = "无中生有";
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
