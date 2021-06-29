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
    public abstract class ZhudongSkillBase : SkillBase, IZhudongSkill
    {
        protected ZhudongSkillBase()
        {
        }

        public async Task Trigger()
        {
            throw new NotImplementedException();
        }

        public bool IsTriggerable()
        {
            throw new NotImplementedException();
        }
    }
}
