using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Player
{
    public class PlayerDistance
    {
        /// <summary>
        /// 探囊取物的距离,计算进攻马、防御马
        /// </summary>
        public int TannangDistance { get; set; }

        /// <summary>
        /// 计算src有武器的杀的距离
        /// </summary>
        public int ShaDistance { get; set; }

        /// <summary>
        /// 不计算src武器的情况下的距离。
        /// </summary>
        public int ShaDistanceWithoutWeapon { get; set; }
    }
}
