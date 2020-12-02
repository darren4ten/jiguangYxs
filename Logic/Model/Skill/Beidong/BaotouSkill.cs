using Logic.Model.Interface.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Skill.Beidong
{
    /// <summary>
    /// 豹子头
    /// </summary>
    public class BaotouSkill : BeidongSkillBase
    {
        public BaotouSkill()
        {
            Name = "Baotou";
            this.DisplayName = "豹子头";
            Description = "出牌时，当出现以下两种情况时你对别的角色出的【杀】不可回避：（1）目标角色的手牌数大于或等于你的血量值。（2）目标角色的手牌数小于或等于你的攻击范围。";
        }
    }
}
