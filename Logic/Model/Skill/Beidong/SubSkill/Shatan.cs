using Logic.Model.Enums;
using Logic.Model.Player;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Model.Skill.Beidong.SubSkill;
using Logic.Model.Skill.Interface;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Skill.SubSkill
{
    /// <summary>
    /// 杀之贪
    /// </summary>
    public class Shatan : SubMainSkillBase
    {
        public Shatan(int star, int level) : base(star, level)
        {
            Name = "shatan";
            DisplayName = "杀之贪";
        }

        public override Task SetupEventListeners()
        {
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.
             ListenEvent(Guid.NewGuid(), PlayerHero, Logic.Model.Enums.EventTypeEnum.AfterShaSuccess, (
                 async (reqContext, roundContext, responseContext) =>
                 {
                     if (ShouldTrigger())
                     {
                         reqContext.SrcPlayer.PickCard(1);
                         Console.WriteLine("触发杀贪");
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
