using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.Skill.Interface;

namespace Logic.Model.Skill.Zhudong
{
    /// <summary>
    /// 主动技能
    /// </summary>
    public abstract class BeidongSkillBase : SkillBase, IBeidongSkill
    {
        protected BeidongSkillBase()
        {
        }
        public Task SetupEventListeners()
        {
            throw new NotImplementedException();
        }
    }
}
