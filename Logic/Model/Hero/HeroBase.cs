using System.Collections.Generic;
using Logic.Enums;
using Logic.GameLevel;
using Logic.Model.Enums;
using Logic.Model.Skill;

namespace Logic.Model.Hero
{
    public abstract class HeroBase : IHero
    {
        public int Id { get; set; }
        public HeroGroupEnum HeroGroup { get; set; }
        /// <summary>
        /// 英雄名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 最大生命值
        /// </summary>
        public int MaxLife { get; set; }

        /// <summary>
        /// 攻击属性
        /// </summary>
        protected AttackDynamicFactor AttackFactor { get; set; }

        public string Image { get; set; }

        /// <summary>
        /// 主技能集合
        /// </summary>
        public List<SkillBase> MainSkillSet { get; set; }

        /// <summary>
        /// 副技能集合
        /// </summary>
        public List<SkillBase> SubSkillSet { get; set; }

        public HeroBase()
        {
            AttackFactor = new AttackDynamicFactor()
            {
                DefenseDistance = 0,
                TannangDistance = 1,
                IsShaNotAvoidable = false,
                MaxCardCountInHand = MaxLife,
                MaxLife = 3,
                MaxShaTargetCount = 1,
                MaxShaTimes = 1,
                PickCardCountPerRound = 2,
                ShaCountAvoidJuedou = 1,
                ShaDistance = 1,
                ShanCountAvoidSha = 1,
                Damage = new Damage()
                {
                    FenghuolangyanDamage = 1,
                    JuedouDamage = 1,
                    ShaDamage = 1,
                    WanjianqifaDamage = 1
                },
                SkipOption = new SkipOption()
                {
                    ShouldSkipEnterMyRound = false,
                    ShouldSkipPickCard = false,
                    ShouldSkipPlayCard = false,
                    ShouldSkipThrowCard = false,
                }
            };
        }

        public AttackDynamicFactor GetBaseAttackFactor()
        {
            return AttackFactor;
        }
    }
}
