using Logic.GameLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Skill
{
    public class SkillContext
    {
        public Logic.Player.Player CurrentPlayer { get; set; }

        public GameLevelBase GameLevel { get; set; }
    }
}
