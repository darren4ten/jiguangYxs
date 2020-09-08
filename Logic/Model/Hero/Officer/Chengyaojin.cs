using Logic.Enums;
using Logic.Hero;
using Logic.Model.Enums;
using Logic.Model.Skill;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Hero.Presizdent
{
    public class Chengyaojin : HeroBase
    {
        public Chengyaojin()
        {
            Name = "Chengyaojin";
            DisplayName = "程咬金";
            Life = 4;
            this.Group = HeroGroupEnum.Officer;
            this.Gender = GenderEnum.Male;
            this.SubSkillSet = new List<SkillBase>();
            this.MainSkillSet = new List<SkillBase>();
        }
    }
}
