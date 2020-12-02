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
            this.CardType = Logic.Enums.CardTypeEnum.Base;
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
