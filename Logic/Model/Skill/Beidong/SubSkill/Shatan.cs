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
using Logic.Log;

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

        public override async Task SetupEventListeners()
        {
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.
             ListenEvent(Guid.NewGuid(), PlayerHero, Logic.Model.Enums.EventTypeEnum.AfterShaSuccess, (
                 async (reqContext, roundContext, responseContext) =>
                 {
                     Console.WriteLine("check杀贪");
                     if (ShouldTrigger())
                     {
                         PlayerHero.PlayerContext.GameLevel.LogManager.LogAction(
                              new RichTextParagraph(
                              new RichTextWrapper(PlayerHero.PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                              new RichTextWrapper("触发"),
                              new RichTextWrapper("杀贪", RichTextWrapper.GetColor(ColorEnum.Red)),
                              new RichTextWrapper("。")
                           ));
                         await PlayerHero.PlayerContext.Player.PickCard(1);
                         Console.WriteLine("触发杀贪");
                     }
                     await Task.FromResult(0);
                 }));
            await Task.FromResult(0);
        }


        public override SkillTypeEnum SkillType()
        {
            return SkillTypeEnum.SubSkill;
        }
    }
}
