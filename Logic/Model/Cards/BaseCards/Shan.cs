using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.BaseCards
{
    /// <summary>
    /// 闪
    /// </summary>
    public class Shan : CardBase
    {
        public Shan()
        {
            this.Description = "闪";
            this.Name = "Shan";
            this.DisplayName = "闪";
        }

        /// <summary>
        /// 闪不能被主动打出
        /// </summary>
        /// <returns></returns>
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
