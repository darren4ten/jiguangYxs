using Logic.Hero;
using Logic.User.UserHero;
using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Player;

namespace Logic.Player
{
    public class Player
    {
        public int PlayerId { get; set; }

        public int PlayerIndex { get; set; }

        public UserHero UserHero { get; set; }
        public bool IsMyRound { get; set; }

        public int GetDefenseDistance()
        {
            return UserHero.GetDefenseDistance();
        }

        public bool IsPlayerAlive()
        {
            return UserHero.IsHeroAlive();
        }

        #region MyRegion

        public Result EnterMyRound()
        {

            return new Result();
        }
  

        public Result PickCard()
        {

            return new Result();
        }
        public Result PlayCard()
        {

            return new Result();
        }

        public Result ThrowCard()
        {

            return new Result();
        }

        public Result ExitMyRound()
        {

            return new Result();
        }
        #endregion
    }
}
