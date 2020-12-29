using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Enums
{
    public enum CardChangeTypeEnum
    {
        /// <summary>
        /// 卡牌一变一。如红色牌当做杀
        /// </summary>
        Changed,
        /// <summary>
        /// 组合变化，比如两张牌当杀
        /// </summary>
        Combined,
    }
}
