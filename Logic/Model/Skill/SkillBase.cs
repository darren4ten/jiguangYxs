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
        public virtual bool CanProvideSha()
        {
            return false;
        }

        public bool CanProvideFuidichouxin()
        {
            return false;
        }

        public bool CanProvideJiedaosharen()
        {
            return false;
        }

        public virtual bool CanProvideShan()
        {
            return false;
        }

        public virtual bool CanProvideYao()
        {
            return false;
        }

        public virtual bool CanProviderWuxiekeji()
        {
            return false;
        }

        public virtual bool CanProvideJuedou()
        {
            return false;
        }

        public virtual bool CanProvideFenghuolangyan()
        {
            return false;
        }

        public virtual bool CanProvideWanjianqifa()
        {
            return false;
        }

        public virtual bool CanProvideTannangquwu()
        {
            return false;
        }

        public bool CanProvideWuzhongshengyou()
        {
            return false;
        }

        public bool CanProvideHudadiweilao()
        {
            return false;
        }

        public bool CanProvideXiuyangshengxi()
        {
            return false;
        }

        #endregion

    }
}
