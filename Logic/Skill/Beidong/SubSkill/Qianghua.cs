using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Log;
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
                ListenEvent(Guid.NewGuid(), PlayerHero, Logic.Model.Enums.EventTypeEnum.BeforeShaSuccess, (
                    async (reqContext, roundContext, responseContext) =>
                    {
                        if (ShouldTrigger())
                        {
                            reqContext.AttackDynamicFactor =
                                reqContext.AttackDynamicFactor ?? AttackDynamicFactor.GetDefaultBaseAttackFactor();
                            reqContext.AttackDynamicFactor.Damage.ShaDamage++;
                            PlayerHero.PlayerContext.GameLevel.LogManager.LogAction(
                               new RichTextParagraph(
                               new RichTextWrapper(PlayerHero.PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                               new RichTextWrapper("触发"),
                               new RichTextWrapper("强化", RichTextWrapper.GetColor(ColorEnum.Red)),
                               new RichTextWrapper("。")
                            ));
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
