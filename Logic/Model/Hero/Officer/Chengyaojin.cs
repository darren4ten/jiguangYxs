using System.Collections.Generic;
using Logic.Enums;
using Logic.Model.Enums;
using Logic.Model.Skill;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Hero.Officer
{
    /// <summary>
    /// 程咬金
    /// </summary>
    public class Chengyaojin : HeroBase
    {
        public Chengyaojin()
        {
            Name = "Chengyaojin";
            DisplayName = "程咬金";
            MaxLife = 4;
            this.HeroGroup = HeroGroupEnum.Officer;
            this.Gender = GenderEnum.Male;
            this.SubSkillSet = new List<SkillBase>();
            this.MainSkillSet = new List<SkillBase>() { new SanbanfuSkill() };
        }
    }
}
