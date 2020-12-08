using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.Enums;
using Logic.Model.Skill.Beidong.SubSkill;
using Logic.Model.Skill.Interface;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Skill.SubSkill
{
    /// <summary>
    /// 强化
    /// </summary>
    public class Qianghua : SubMainSkillBase
    {
        public Qianghua(int star, int level) : base(star, level)
        {
            Name = "Qianghua";
            DisplayName = "强化";
        }

        public override Task SetupEventListeners()
        {
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.
                ListenEvent(Guid.NewGuid(), PlayerHero, Logic.Model.Enums.EventTypeEnum.AfterShaSuccess, (
                    async (reqContext, roundContext, responseContext) =>
                    {
                        if (ShouldTrigger())
                        {
                            reqContext.TargetPlayers.PickCard(1);
                            Console.WriteLine("触发强化");
                        }

                        await Task.FromResult("");
                    }));
            return Task.FromResult("");
        }


        public override SkillTypeEnum SkillType()
        {
            return SkillTypeEnum.SubSkill;
        }
    }
}
