using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Enums;
using Logic.Model.Skill.Interface;

namespace Logic.Model.Skill.Zhudong
{
    /// <summary>
    /// 武穆
    /// </summary>
    public class WumuSkill : ZhudongSkillBase, ISkillButton
    {
        public WumuSkill()
        {
            Name = "Wumu";
            DisplayName = "武穆";
        }

        /// <summary>
        /// 能出杀或者出闪的时候出牌
        /// </summary>
        /// <returns></returns>
        public bool IsEnabled()
        {
            return Sha.CanBePlayed(PlayerHero.PlayerContext) || Shan.CanBePlayed(PlayerHero.PlayerContext);
        }

        public SkillButtonInfo GetButtonInfo()
        {
            return new SkillButtonInfo()
            {
                Text = "武穆",
                SkillType = SkillType(),
                Description = "闪可当做杀、杀可当做闪使用",
                OnClick = async (context, roundContext, responseContext) => { await Task.FromResult(0); }
            };
        }
    }
}
