using Logic.Enums;
using Logic.Model.Enums;
using Logic.Model.Skill;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Hero
{
    public abstract class HeroBase
    {
        public HeroGroupEnum Group { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public GenderEnum Gender { get; set; }
        public int Life { get; set; }

        public int AttackDistance { get; set; }

        public int TannangDistance { get; set; }
        public int MaxShaCount { get; set; } = 1;

        public string Image { get; set; }
        public List<SkillBase> MainSkillSet { get; set; }
        public List<SkillBase> SubSkillSet { get; set; }
    }
}
