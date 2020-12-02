using Logic.Enums;
using Logic.Model.Enums;
using Logic.Model.Skill;
using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Hero.Presizdent
{
    /// <summary>
    /// 项羽
    /// </summary>
    public class Xiangyu : HeroBase
    {
        public Xiangyu()
        {
            Name = "Xiangyu";
            DisplayName = "项羽";
            MaxLife = 4;
            this.HeroGroup = HeroGroupEnum.Presizdent;
            this.Gender = GenderEnum.Male;
            this.SubSkillSet = new List<SkillBase>()
            {

            };
            this.MainSkillSet = new List<SkillBase>()
            {
                new BawangSkill()
            };
        }
    }
}
