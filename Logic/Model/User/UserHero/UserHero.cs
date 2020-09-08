using Logic.Cards;
using Logic.Hero;
using Logic.Model.Cards.EquipmentCards;
using Logic.Model.Skill;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Logic.Cards.JinlangCards;

namespace Logic.User.UserHero
{
    public class UserHero
    {
        public int Id { get; set; }

        public int CurrentLife { get; set; }
        public HeroBase Hero { get; set; }
        public List<EquipmentCardBase> EquipmentSet { get; set; }
        public List<SkillBase> ExtraMainSkillSet { get; set; }
        public List<SkillBase> ExtraSubSkillSet { get; set; }

        public int Star { get; set; } = 1;

        public int Yuanshen { get; set; }

        public int ShaedCount { get; set; }

        public int GetMaxShaCount()
        {
            var eqCount = EquipmentSet.Sum(e => e.MaxShaCount);
            return Hero.MaxShaCount + eqCount;
        }

        public int GetMaxLife()
        {
            return this.Hero.Life + (Star - 1);
        }

        public bool IsHeroAlive()
        {
            return true;
        }

        public int GetDefenseDistance()
        {
            var distMa = EquipmentSet.FirstOrDefault(p => p is Fangyuma);
            return distMa == null ? 0 : 1;
        }

        public int GetMaxAttackDistance()
        {
            //获取装备的攻击距离
            var distMa = EquipmentSet.FirstOrDefault(p => p is Jingongma)?.AttackDistance ?? 0;
            var distEqp = EquipmentSet.Where(p => !(p is Jingongma)).Max(p => p.AttackDistance);
            return this.Hero.AttackDistance + distMa + distEqp;
        }
    }
}
