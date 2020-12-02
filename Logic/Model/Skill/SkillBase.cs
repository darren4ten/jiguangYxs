using Logic.Model.Enums;
using Logic.Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.Skill.Interface;

namespace Logic.Model.Skill
{
    public abstract class SkillBase : ISkill, IAbility
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }
        public string Description { get; set; }
        public SkillTypeEnum SkillType { get; set; }

        public Task LoadSkill()
        {
            throw new NotImplementedException();
        }

        public Task UnLoadSkill()
        {
            throw new NotImplementedException();
        }

        SkillTypeEnum ISkill.SkillType()
        {
            throw new NotImplementedException();
        }

        #region IAbility
        public bool CanProvideSha()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideShan()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideJuedou()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideFenghuolangyan()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideWanjianqifa()
        {
            throw new NotImplementedException();
        }

        public bool CanProvideTannangquwu()
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
