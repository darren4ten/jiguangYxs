using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Enums;

namespace Logic.Model.Skill.Zhudong
{
    /// <summary>
    /// 傲剑
    /// </summary>
    public class AojianSkill : ZhudongSkillBase
    {
        public AojianSkill()
        {
            SkillType = SkillTypeEnum.MainSkill;
            Name = "Aojian";
            DisplayName = "傲剑";
        }
    }
}
