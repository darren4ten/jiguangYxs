using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Enums;

namespace Logic.Model.Skill.Zhudong
{
    /// <summary>
    /// 三板斧
    /// </summary>
    public class SanbanfuSkill : ZhudongSkillBase
    {
        public SanbanfuSkill()
        {
            SkillType = SkillTypeEnum.MainSkill;
            Name = "Sanbanfu";
            DisplayName = "三板斧";
        }
    }
}
