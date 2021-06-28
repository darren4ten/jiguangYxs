using Logic.Enums;
using Logic.Model.Enums;
using Logic.Model.Skill;
using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Skill.Beidong;

namespace Logic.Model.Hero.Presizdent
{
    /// <summary>
    /// 朱元璋
    /// </summary>
    public class Zhuyuanzhang : HeroBase
    {
        public Zhuyuanzhang()
        {
            Name = "Zhuyuanzhang";
            DisplayName = "朱元璋";
            MaxLife = 4;
            this.HeroGroup = HeroGroupEnum.Presizdent;
            this.Gender = GenderEnum.Male;
            this.SubSkillSet = new List<SkillBase>();
            this.MainSkillSet = new List<SkillBase>()
            {
                new QiangyunSkill()
            };
        }
    }
}
