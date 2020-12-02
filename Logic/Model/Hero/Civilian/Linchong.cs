using System.Collections.Generic;
using Logic.Enums;
using Logic.Model.Enums;
using Logic.Model.Skill;
using Logic.Model.Skill.Beidong;

namespace Logic.Model.Hero.Civilian
{
    /// <summary>
    /// 林冲
    /// </summary>
    public class Linchong : HeroBase
    {
        public Linchong()
        {
            Name = "Linchong";
            DisplayName = "林冲";
            MaxLife = 4;
            this.HeroGroup = HeroGroupEnum.Civilian;
            this.Gender = GenderEnum.Male;
            this.SubSkillSet = new List<SkillBase>();
            this.MainSkillSet = new List<SkillBase>() { new BaotouSkill() };
        }
    }
}
