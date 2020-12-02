using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Skill.Beidong.SubSkill;
using Logic.Model.Skill.Interface;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Skill.SubSkill
{
    /// <summary>
    /// 强化
    /// </summary>
    public class Qianghua : SubMainSkillBase
    {
        public Qianghua(int star, int level) : base(star, level)
        {
            Name = "Qianghua";
            DisplayName = "强化";
        }
    }
}
