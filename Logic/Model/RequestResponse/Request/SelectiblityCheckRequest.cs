using System;
using System.Collections.Generic;
using System.Text;
using Logic.Cards;
using Logic.Model.Enums;

namespace Logic.Model.RequestResponse.Request
{
    public class SelectiblityCheckRequest : BaseRequest
    {
        /// <summary>
        /// 选择目标的类型
        /// </summary>
        public AttackTypeEnum TargetType { get; set; }

        /// <summary>
        /// 请求来源玩家
        /// </summary>
        public Player.Player SrcPlayer { get; set; }

        /// <summary>
        /// 来源牌
        /// </summary>
        public List<CardBase> SrcCards { get; set; }
    }
}
