using Logic.Model.Enums;
using Logic.Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.Player;
using Logic.Model.Skill.Interface;

namespace Logic.Model.Skill
{
    public abstract class SkillBase : ISkill, IAbility
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }
        public string Description { get; set; }

        protected PlayerHero PlayerHero { get; set; }

        public virtual Task LoadSkill(PlayerHero playerHero)
        {
            PlayerHero = playerHero;
            return Task.FromResult("");
        }

        public virtual Task UnLoadSkill()
        {
            throw new NotImplementedException();
        }

        public virtual SkillTypeEnum SkillType()
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
