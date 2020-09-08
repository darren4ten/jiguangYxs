using Logic.Model.Skill;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Interface
{
    public interface ISkill
    {
        bool IsSkillClickable();
        bool IsSkillTriggerable();
    }
}
