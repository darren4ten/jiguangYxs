using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Enums;

namespace Logic.Model.Skill.Zhudong
{
    /// <summary>
    /// 霸王
    /// </summary>
    public class BawangSkill : BeidongSkillBase
    {
        public BawangSkill()
        {
            SkillType = SkillTypeEnum.MainSkill;
            Name = "Bawang";
            DisplayName = "霸王";
        }
    }
}
