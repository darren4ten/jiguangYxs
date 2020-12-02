using System.Collections.Generic;
using Logic.Enums;
using Logic.Model.Enums;
using Logic.Model.Skill;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Hero.Civilian
{
    /// <summary>
    /// 岳飞
    /// </summary>
    public class Yuefei : HeroBase
    {
        public Yuefei()
        {
            Name = "Yuefei";
            DisplayName = "岳飞";
            MaxLife = 4;
            this.HeroGroup = HeroGroupEnum.Officer;
            this.Gender = GenderEnum.Male;
            this.SubSkillSet = new List<SkillBase>();
            this.MainSkillSet = new List<SkillBase>()
            {
                new WumuSkill()
            };
        }
    }
}
