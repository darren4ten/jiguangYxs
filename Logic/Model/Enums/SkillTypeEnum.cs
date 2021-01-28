using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Logic.Model.Enums
{
    public enum SkillTypeEnum
    {
        /// <summary>
        /// 主技能
        /// </summary>
        MainSkill,
        /// <summary>
        /// 副技能
        /// </summary>
        SubSkill,
        /// <summary>
        /// 玉如意
        /// </summary>
        [Description("玉如意")]
        Yuruyi,
        /// <summary>
        /// 盘龙棍
        /// </summary>
        [Description("盘龙棍")]
        Panlonggun,
        /// <summary>
        /// 博浪锤弃牌命中的技能
        /// </summary>
        [Description("博浪锤")]
        Bolangchui,
        /// <summary>
        /// 芦叶枪
        /// </summary>
        [Description("芦叶枪")]
        Luyeqiang,
        /// <summary>
        /// 龙鳞刀
        /// </summary>
        [Description("龙鳞刀")]
        Longlindao,
        /// <summary>
        /// 红颜
        /// </summary>
        [Description("红颜")]
        Hongyan,
        /// <summary>
        /// 醉酒
        /// </summary>
        [Description("醉酒")]
        Zuijiu,
        /// <summary>
        /// 集权
        /// </summary>
        [Description("集权")]
        Jiquan,
        /// <summary>
        /// 反击
        /// </summary>
        [Description("反击")]
        Fanji,
        /// <summary>
        /// 攻心
        /// </summary>
        [Description("攻心")]
        Gongxin,
        /// <summary>
        /// 毒计
        /// </summary>
        [Description("毒计")]
        Duji,
        /// <summary>
        /// 疏财
        /// </summary>
        [Description("疏财")]
        Shucai,
        /// <summary>
        /// 三板斧
        /// </summary>
        [Description("三板斧")]
        SanBanfu,
        /// <summary>
        /// 武穆
        /// </summary>
        [Description("武穆")]
        Wumu,
        /// <summary>
        /// 傲剑
        /// </summary>
        [Description("傲剑")]
        Aojian
    }
}
