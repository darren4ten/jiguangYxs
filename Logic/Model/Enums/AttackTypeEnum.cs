using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Logic.Model.Enums
{
    public enum AttackTypeEnum
    {
        None,
        Sha,
        [Description("决斗")]
        Juedou,
        [Description("烽火狼烟")]
        Fenghuolangyan,
        [Description("万箭齐发")]
        Wanjianqifa,
        Xiadan,
        Hongyan,
        Gongxin,
        [Description("手捧雷")]
        Shoupenglei,
        /// <summary>
        /// 求药
        /// </summary>
        [Description("求药")]
        Qiuyao,
        /// <summary>
        /// 群体请求（需要确认）
        /// </summary>
        GroupRequestWithConfirm,
        /// <summary>
        /// 无中生有
        /// </summary>
        [Description("无中生有")]
        Wuzhongshengyou,
        /// <summary>
        /// 无懈可击
        /// </summary>
        [Description("无懈可击")]
        Wuxiekeji,
        /// <summary>
        /// 借刀杀人
        /// </summary>
        [Description("借刀杀人")]
        Jiedaosharen,
        /// <summary>
        /// 釜底抽薪
        /// </summary>
        [Description("釜底抽薪")]
        Fudichouxin,
        /// <summary>
        /// 五谷丰登
        /// </summary>
        [Description("五谷丰登")]
        Wugufengdeng,
        /// <summary>
        /// 休养生息
        /// </summary>
        [Description("休养生息")]
        Xiuyangshengxi,
        /// <summary>
        /// 龙鳞刀卸牌
        /// </summary>
        [Description("龙鳞刀")]
        Longlindao,
        /// <summary>
        /// 芦叶枪
        /// </summary>
        [Description("芦叶枪")]
        Luyeqiang,
        /// <summary>
        /// 画地为牢
        /// </summary>
        [Description("画地为牢")]
        Huadiweilao,
        [Description("探囊取物")]
        Tannangquwu,
        [Description("弃牌")]
        ThrowCard,
        /// <summary>
        /// 博浪锤
        /// </summary>
        [Description("博浪锤")]
        Bolangchui,
        /// <summary>
        /// 盘龙棍
        /// </summary>
        [Description("盘龙棍")]
        Panlonggun,
        /// <summary>
        /// 请求选牌
        /// </summary>
        SelectCard,
        /// <summary>
        /// 疗伤
        /// </summary>
        [Description("疗伤")]
        Liaoshang,
        /// <summary>
        /// 治愈
        /// </summary>
        [Description("治愈")]
        Zhiyu,
    }
}
