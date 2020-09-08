using Logic.Enums;
using Logic.GameLevel;

namespace Logic.Cards
{
    public class CardContext
    {
        public Player.Player FromPlayer { get; set; }
        public Player.Player ToPlayer { get; set; }

        public RequestContext RequestContext { get; set; }
    }
}
