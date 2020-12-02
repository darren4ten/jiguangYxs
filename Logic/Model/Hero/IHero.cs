using System;
using System.Collections.Generic;
using System.Text;
using Logic.GameLevel;

namespace Logic.Model.Hero
{
    public interface IHero
    {
        /// <summary>
        /// 获取Hero的攻击加成
        /// </summary>
        /// <returns></returns>
        AttackDynamicFactor GetBaseAttackFactor();
    }
}
