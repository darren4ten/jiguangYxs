using Logic.Model.Hero.Civilian;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;
using System.Collections.Generic;

namespace Logic.GameLevel.Levels
{
    public class GameLevel1 : GameLevelBase
    {
        public GameLevel1()
        {
            Name = "Level 1";
            Description = "Level 1 test.";
        }

        protected override void LoadPlayers(Player currentPlayer, List<Player> aditionalPlayers)
        {
            base.LoadPlayers(currentPlayer, aditionalPlayers);
        }
    }
}
