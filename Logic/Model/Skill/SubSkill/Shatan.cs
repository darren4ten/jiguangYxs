using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Skill.SubSkill
{
    public class Shatan : SkillBase
    {
        public Shatan(SkillContext SkillContext) : base(SkillContext)
        {
        }

        public override bool IsSkillClickable()
        {
            return base.IsSkillClickable();
        }

        public override bool IsSkillTriggerable()
        {
            return base.IsSkillTriggerable();
        }
    }
}
