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
    /// 三板斧
    /// </summary>
    public class SanbanfuSkill : ZhudongSkillBase, ISkillButton, IEnhanceSha
    {

        public SanbanfuSkill()
        {
            Name = "Sanbanfu";
            DisplayName = "三板斧";
        }

        public bool IsEnabled()
        {
            //如果可以出杀且技能触发次数<1才能enable
            if (Sha.CanBePlayed(PlayerHero.PlayerContext))
            {
                if (PlayerHero.PlayerContext.Player.RoundContext == null)
                {
                    return false;
                }
                var containsSkill = PlayerHero.PlayerContext.Player.RoundContext.SkillTriggerTimesDic?.ContainsKey(SkillTypeEnum.SanBanfu);
                if (containsSkill == null)
                {
                    PlayerHero.PlayerContext.Player.RoundContext.SkillTriggerTimesDic = new Dictionary<SkillTypeEnum, int>();
                    return true;
                }

                if (containsSkill == false)
                {
                    return true;
                }

                if (PlayerHero.PlayerContext.Player.RoundContext.SkillTriggerTimesDic[SkillTypeEnum.SanBanfu] < 1)
                {
                    return true;
                }
            }

            return false;
        }

        public SkillButtonInfo GetButtonInfo()
        {
            return new SkillButtonInfo()
            {
                Text = "三板斧",
                Description = "",
                SkillType = SkillType(),
                OnClick = async (context, roundContext, responseContext) =>
                {
                    //触发时，提示选择一个攻击目标
                    await Task.FromResult(0);
                }
            };
        }

        public int EnhancePriority()
        {
            return 100;
        }
    }
}
