using Logic.Enums;
using Logic.Hero;
using Logic.Model.Enums;
using Logic.Model.Skill;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Hero.Presizdent
{
    public class Zhuyuanzhang : HeroBase
    {
        public Zhuyuanzhang()
        {
            Name = "Zhuyuanzhang";
            DisplayName = "朱元璋";
            Life = 4;
            this.Group = HeroGroupEnum.Presizdent;
            this.Gender = GenderEnum.Male;
            this.SubSkillSet = new List<SkillBase>();
            this.MainSkillSet = new List<SkillBase>();
        }
    }
}
