using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Logic.Model.Enums
{
    public enum DamageTypeEnum
    {
        [Description("无")]
        None,
        [Description("杀")]
        Sha,
        [Description("决斗")]
        Juedou,
        [Description("烽火狼烟")]
        Fenghuolangyan,
        [Description("万箭齐发")]
        Wanjianqifa,
        [Description("三板斧")]
        Sanbanfu,
        [Description("攻心")]
        Gongxin,
        [Description("毒计")]
        Duji,
        [Description("手捧雷")]
        Shoupenglei
    }
}
