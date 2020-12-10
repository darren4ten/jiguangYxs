using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.GameLevel
{
    /// <summary>
    /// 攻击加成（一般指增量）
    /// </summary>
    public class AttackDynamicFactor
    {
        /// <summary>
        /// 最大血量（增量）
        /// </summary>
        public int MaxLife { get; set; }

        /// <summary>
        /// 最大出杀的次数
        /// </summary>
        public int MaxShaTimes { get; set; }

        /// <summary>
        /// 最大杀的目标数
        /// </summary>
        public int MaxShaTargetCount { get; set; }

        /// <summary>
        /// 无中生有摸牌的卡牌数量
        /// </summary>
        public int WuzhongshengyouCardCount { get; set; }

        /// <summary>
        /// 最大探囊距离
        /// </summary>
        public int TannangDistance { get; set; }

        /// <summary>
        /// 防御距离
        /// </summary>  
        public int DefenseDistance { get; set; }

        /// <summary>
        /// 杀的攻击距离
        /// </summary>
        public int ShaDistance { get; set; }

        /// <summary>
        /// 回合摸牌数
        /// </summary>
        public int PickCardCountPerRound { get; set; }

        /// <summary>
        /// 最大持有手牌数（弃牌的时候检查）
        /// </summary>
        public int MaxCardCountInHand { get; set; }

        /// <summary>
        /// 闪避杀需要出闪的数量
        /// </summary>
        public int ShanCountAvoidSha { get; set; }

        /// <summary>
        /// 闪避决斗需要出杀的数量
        /// </summary>
        public int ShaCountAvoidJuedou { get; set; }

        /// <summary>
        /// 杀是否不可闪避
        /// </summary>
        public bool IsShaNotAvoidable { get; set; }

        public Recover Recover { get; set; }
        /// <summary>
        /// 伤害
        /// </summary>
        public Damage Damage { get; set; }

        public SkipOption SkipOption { get; set; }

        /// <summary>
        /// 获取默认的英雄攻击相关的基础信息（非增量）
        /// </summary>
        /// <returns></returns>
        public static AttackDynamicFactor GetDefaultBaseAttackFactor()
        {
            return new AttackDynamicFactor()
            {
                DefenseDistance = 0,
                TannangDistance = 1,
                WuzhongshengyouCardCount = 2,
                IsShaNotAvoidable = false,
                MaxCardCountInHand = 3,
                MaxLife = 3,
                MaxShaTargetCount = 1,
                MaxShaTimes = 1,
                PickCardCountPerRound = 2,
                ShaCountAvoidJuedou = 1,
                ShaDistance = 1,
                ShanCountAvoidSha = 1,
                Recover = new Recover()
                {
                    XiuyangshengxiLife = 1,
                    XixueLife = 1,
                    YaoLife = 1
                },
                Damage = new Damage()
                {
                    FenghuolangyanDamage = 1,
                    JuedouDamage = 1,
                    ShaDamage = 1,
                    WanjianqifaDamage = 1,
                    GongxinDamage = 1,
                    DujiDamage = 1,
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

        /// <summary>
        /// 获取默认的英雄攻击相关的基础信息（增量）
        /// </summary>
        /// <returns></returns>
        public static AttackDynamicFactor GetDefaultDeltaAttackFactor()
        {
            return new AttackDynamicFactor()
            {
                DefenseDistance = 0,
                TannangDistance = 0,
                WuzhongshengyouCardCount = 0,
                IsShaNotAvoidable = false,
                MaxCardCountInHand = 0,
                MaxLife = 0,
                MaxShaTargetCount = 0,
                MaxShaTimes = 0,
                PickCardCountPerRound = 0,
                ShaCountAvoidJuedou = 0,
                ShaDistance = 0,
                ShanCountAvoidSha = 0,
                Damage = new Damage()
                {
                    FenghuolangyanDamage = 0,
                    JuedouDamage = 0,
                    ShaDamage = 0,
                    WanjianqifaDamage = 0
                },
                Recover = new Recover()
                {
                    XiuyangshengxiLife = 0,
                    XixueLife = 0,
                    YaoLife = 0
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
    }

    public class SkipOption
    {
        /// <summary>
        /// 需要跳过弃牌
        /// </summary>
        public bool ShouldSkipThrowCard { get; set; }

        /// <summary>
        /// 需要跳过摸牌
        /// </summary>
        public bool ShouldSkipPickCard { get; set; }

        /// <summary>
        /// 需要跳过我的回合
        /// </summary>
        public bool ShouldSkipEnterMyRound { get; set; }

        /// <summary>
        /// 需要跳过出牌
        /// </summary>
        public bool ShouldSkipPlayCard { get; set; }
    }

    /// <summary>
    /// 恢复相关参数
    /// </summary>
    public class Recover
    {
        /// <summary>
        /// 休养生息回复的生命值
        /// </summary>
        public int XiuyangshengxiLife { get; set; }

        /// <summary>
        /// 吃药回复的生命值
        /// </summary>
        public int YaoLife { get; set; }

        /// <summary>
        /// 吸血回复的生命值
        /// </summary>
        public int XixueLife { get; set; }
    }
    public class Damage
    {
        /// <summary>
        /// 杀的伤害
        /// </summary>
        public int ShaDamage { get; set; }

        /// <summary>
        /// 烽火狼烟的伤害
        /// </summary>
        public int FenghuolangyanDamage { get; set; }

        /// <summary>
        /// 万箭齐发的伤害
        /// </summary>
        public int WanjianqifaDamage { get; set; }

        /// <summary>
        /// 决斗的伤害
        /// </summary>
        public int JuedouDamage { get; set; }

        /// <summary>
        /// 攻心的伤害
        /// </summary>
        public int GongxinDamage { get; set; }

        /// <summary>
        /// 毒计的伤害
        /// </summary>
        public int DujiDamage { get; set; }
    }
}
