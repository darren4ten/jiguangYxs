using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Enums;
using Logic.Model.Skill.Interface;

namespace Logic.Model.Skill.Zhudong
{
    /// <summary>
    /// 傲剑
    /// </summary>
    public class AojianSkill : ZhudongSkillBase, ISkillButton
    {
        public AojianSkill()
        {
            Name = "Aojian";
            DisplayName = "傲剑";
        }

        public bool IsEnabled()
        {
            //能够出杀的时候
            return Sha.CanBePlayed(PlayerHero.PlayerContext);
        }

        public override SkillTypeEnum SkillType()
        {
            return SkillTypeEnum.Aojian;
        }
        public SkillButtonInfo GetButtonInfo()
        {
            return new SkillButtonInfo()
            {
                Text = "傲剑",
                Description = "红色牌可以当做杀",
                SkillType = SkillType(),
                OnClick = async (context, roundContext, responseContext) =>
                {
                    await Task.FromResult(0);
                }
            };
        }
    }
}
