using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Enums;

namespace Logic.GameLevel
{
    public class RoundContext
    {
        /// <summary>
        /// 攻击加成
        /// </summary>
        public AttackDynamicFactor AttackDynamicFactor { get; set; }

        public int RoundId { get; set; }

        /// <summary>
        /// 回合中各个技能触发次数字典
        /// </summary>
        public Dictionary<SkillTypeEnum, int> SkillTriggerTimesDic { get; set; }
    }
}
