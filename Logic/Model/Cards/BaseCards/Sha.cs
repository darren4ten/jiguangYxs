using System.Threading.Tasks;
using Logic.Cards;

namespace Logic.Model.Cards.BaseCards
{
    /// <summary>
    /// 杀
    /// </summary>
    public class Sha : CardBase
    {
        public Sha()
        {
            this.Description = "杀";
            this.Name = "Sha";
            this.DisplayName = "杀";
            this.CardType = Logic.Enums.CardTypeEnum.Base;
        }

        public override bool CanBePlayed()
        {
            //1.主动出杀

            //2. 被动出杀
            return false;
        }

        public override Task Popup()
        {
            throw new System.NotImplementedException();
        }
    }
}
