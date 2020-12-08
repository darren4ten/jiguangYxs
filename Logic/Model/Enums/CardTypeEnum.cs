using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Enums
{
    public enum CardTypeEnum
    {
        Any,
        /// <summary>
        /// 只限手牌
        /// </summary>
        InHand,
        /// <summary>
        /// 只限装备牌
        /// </summary>
        Equipment,
        /// <summary>
        /// 只限武器牌
        /// </summary>
        Weapon,
        /// <summary>
        /// 只限手牌或者装备牌
        /// </summary>
        InHandAndEquipment,
        /// <summary>
        /// 只限防具
        /// </summary>
        Defender,
        /// <summary>
        /// 只限防具和武器
        /// </summary>
        WeaponAndDefender,
        /// <summary>
        /// 仅限进攻马或者防御马
        /// </summary>
        Horse,
        /// <summary>
        /// 只限锦囊牌
        /// </summary>
        Jinlang
    }
}
