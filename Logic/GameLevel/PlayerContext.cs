using System;
using System.Collections.Generic;
using System.Text;
using Logic.ActionManger;
using Logic.Model.Player;

namespace Logic.GameLevel
{
    public class PlayerContext
    {
        public GameLevelBase GameLevel { get; set; }

        public Player Player { get; set; }

        /// <summary>
        /// 是否是AI
        /// </summary>
        /// <returns></returns>
        public bool IsAi()
        {
            return Player.IsAi();
        }
    }
}
