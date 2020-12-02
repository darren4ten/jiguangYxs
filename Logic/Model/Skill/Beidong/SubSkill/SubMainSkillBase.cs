using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Skill.Interface;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Skill.Beidong.SubSkill
{
    public class SubMainSkillBase : BeidongSkillBase, ISubSkill
    {
        /// <summary>
        /// 星级（1-5）
        /// </summary>
        public int Star { get; }

        /// <summary>
        /// 等级（1-50）
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// 触发概率
        /// </summary>
        public double TriggerRate { get; private set; }

        private Dictionary<int, double> TriggerRateTable = new Dictionary<int, double>()
        {
            {1, 5.0},
            {2, 10.0},
            {3, 15.0},
            {4, 20.0},
            {5, 30.0},
        };

        public SubMainSkillBase(int star, int level)
        {
            Star = star;
            Level = level;
            TriggerRate = GetTriggerRate(star, level);
        }

        protected double GetTriggerRate(int star, int level)
        {
            if (TriggerRateTable.ContainsKey(star))
            {
                var baseRate = TriggerRateTable[star];
                return baseRate + level;
            }

            return 0;
        }

        public virtual bool ShouldTrigger()
        {
            return new Random().Next(0, 100) <= TriggerRate;
        }
    }
}
