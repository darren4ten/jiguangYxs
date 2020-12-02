using System;
using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.BaseCards
{
    /// <summary>
    /// 药
    /// </summary>
    public class Yao : CardBase
    {
        public Yao()
        {
            this.Description = "药";
            this.Name = "Yao";
            this.DisplayName = "药";
            this.CardType = Logic.Enums.CardTypeEnum.Base;
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
