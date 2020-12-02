using Logic.Model.Hero.Civilian;
using Logic.Model.Hero.Presizdent;
using Logic.Model.Player;

namespace Logic.GameLevel.Levels
{
    public class GameLevel1 : GameLevelBase
    {
        public GameLevel1()
        {
            Name = "Level 1";
            Description = "Level 1 test.";
        }

        protected override void LoadPlayers()
        {
            //var player1 = new Player()
            //{
            //    PlayerId = 1,
            //    PlayerHero = new PlayerHero()
            //    {
            //        Hero = new Linchong(),
            //        Star = 3,
            //    }
            //};
        }
    }
}
