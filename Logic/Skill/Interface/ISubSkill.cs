using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Skill.Interface
{
    public interface ISubSkill
    {
        /// <summary>
        /// 是否应该触发
        /// </summary>
        /// <returns></returns>
        bool ShouldTrigger();
    }
}
