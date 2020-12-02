using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Enums;

namespace Logic.Model.Skill.Zhudong
{
    /// <summary>
    /// 武穆
    /// </summary>
    public class WumuSkill : ZhudongSkillBase
    {
        public WumuSkill()
        {
            SkillType = SkillTypeEnum.MainSkill;
            Name = "Wumu";
            DisplayName = "武穆";
        }
    }
}
