using System.Collections.Generic;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Enums;

namespace Logic.Model.RequestResponse.Request
{
    /// <summary>
    /// 选择目标
    /// </summary>
    public class SelectedTargetsRequest : BaseRequest
    {
        /// <summary>
        /// 选择目标的类型
        /// </summary>
        public AttackTypeEnum TargetType { get; set; }

        public CardRequestContext CardRequest { get; set; }
        public RoundContext RoundContext { get; set; }

        /// <summary>
        /// 攻击牌
        /// </summary>
        public List<CardBase> SrcCards { get; set; }

        /// <summary>
        /// 最大目标数
        /// </summary>
        public int MaxTargetCount { get; set; }

        /// <summary>
        /// 最小目标数
        /// </summary>
        public int MinTargetCount { get; set; }
    }
}
