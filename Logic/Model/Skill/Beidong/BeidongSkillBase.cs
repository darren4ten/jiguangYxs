using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.Enums;
using Logic.Model.Player;
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

        public override async Task LoadSkill(PlayerHero playerHero)
        {
            await base.LoadSkill(playerHero);
            await SetupEventListeners();
        }

        public Task UnLoadSkill()
        {
            throw new NotImplementedException();
        }

        public virtual SkillTypeEnum SkillType()
        {
            return SkillTypeEnum.MainSkill;
        }

        public virtual Task SetupEventListeners()
        {
            throw new NotImplementedException();
        }
    }
}
