using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Skill.Beidong.SubSkill;
using Logic.Model.Skill.Interface;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Skill.SubSkill
{
    /// <summary>
    /// 吸血
    /// </summary>
    public class Xixue : SubMainSkillBase
    {
        public Xixue(int star, int level) : base(star, level)
        {
            Name = "Xixue";
            DisplayName = "吸血";
        }
    }
}
