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

        /// <summary>
        /// 伤害
        /// </summary>
        public Damage Damage { get; set; }

        public SkipOption SkipOption { get; set; }

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
    }
}
