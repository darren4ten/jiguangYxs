using Logic.Model.Hero.Presizdent;
using Logic.User.UserHero;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.GameLevel
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
            var player1 = new Player.Player()
            {
                PlayerId = 1,
                UserHero = new UserHero()
                {
                    Hero = new Linchong(),
                    Star = 3,
                }
            };
        }
    }
}
