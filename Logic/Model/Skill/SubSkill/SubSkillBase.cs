using System;
using System.Threading.Tasks;

namespace Logic.Model.Skill.SubSkill
{
    /// <summary>
    /// 主动技能
    /// </summary>
    public abstract class SubSkillBase : SkillBase
    {
        protected SubSkillBase()
        {
            SkillType = Enums.SkillTypeEnum.SubSkill;
        }

        public Task Trigger()
        {
            throw new NotImplementedException();
        }

        public bool IsTriggerable()
        {
            throw new NotImplementedException();
        }
    }
}
