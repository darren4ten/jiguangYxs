using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Log;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;
using Logic.Model.Skill.Beidong.SubSkill;
using Logic.Model.Skill.Interface;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Skill.SubSkill
{
    /// <summary>
    /// 吸血
    /// </summary>
    public class Xixue : SubMainSkillBase
    {
        public Xixue(int star, int level) : base(star, level)
        {
            Name = "Xixue";
            DisplayName = "吸血";
        }

        public override Task SetupEventListeners()
        {
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.
                ListenEvent(Guid.NewGuid(), PlayerHero, Logic.Model.Enums.EventTypeEnum.AfterShaSuccess, (
                    async (reqContext, roundContext, responseContext) =>
                    {
                        if (ShouldTrigger())
                        {
                            var triggered = await PlayerHero.PlayerContext.Player.CurrentPlayerHero.AddLife(new AddLifeRequest()
                            {
                                CardResponseContext = responseContext,
                                CardRequestContext = reqContext,
                                SrcRoundContext = roundContext,
                                RequestId = Guid.NewGuid(),
                                RecoverType = RecoverTypeEnum.Xixue
                            });
                            if (triggered)
                            {
                                Console.WriteLine($"【{PlayerHero.Hero.DisplayName}】触发吸血");
                                PlayerHero.PlayerContext.GameLevel.LogManager.LogAction(
                                  new RichTextParagraph(
                                  new RichTextWrapper(PlayerHero.PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                  new RichTextWrapper("触发"),
                                  new RichTextWrapper("吸血", RichTextWrapper.GetColor(ColorEnum.Red)),
                                  new RichTextWrapper("。")
                               ));
                            };
                        }
                    }));
            return Task.FromResult("");
        }


        public override SkillTypeEnum SkillType()
        {
            return SkillTypeEnum.SubSkill;
        }
    }
}
