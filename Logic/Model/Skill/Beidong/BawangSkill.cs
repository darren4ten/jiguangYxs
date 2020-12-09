using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Model.Enums;

namespace Logic.Model.Skill.Zhudong
{
    /// <summary>
    /// 霸王
    /// </summary>
    public class BawangSkill : BeidongSkillBase
    {
        public BawangSkill()
        {
            Name = "Bawang";
            DisplayName = "霸王";
        }

        public override async Task SetupEventListeners()
        {
            PlayerHero.BaseAttackFactor.ShanCountAvoidSha += 1;
            PlayerHero.BaseAttackFactor.ShaCountAvoidJuedou += 1;
            await Task.FromResult(0);
        }
    }
}
