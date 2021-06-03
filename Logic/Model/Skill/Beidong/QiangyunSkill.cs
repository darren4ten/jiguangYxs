using Logic.Model.Interface.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Log;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Skill.Beidong
{
    /// <summary>
    /// 强运
    /// </summary>
    public class QiangyunSkill : BeidongSkillBase
    {
        public QiangyunSkill()
        {
            Name = "Qiangyun";
            this.DisplayName = "强运";
            Description = "当手中无牌时从牌堆顶部摸2张牌";
        }

        public override async Task SetupEventListeners()
        {
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.
                ListenEvent(Guid.NewGuid(), PlayerHero, Logic.Model.Enums.EventTypeEnum.AfterLoseCardsInHand, (
                    async (reqContext, roundContext, responseContext) =>
                    {
                        if (ShouldTrigger())
                        {
                            Console.WriteLine("触发强运");
                            PlayerHero.PlayerContext.GameLevel.LogManager.LogAction(
                                new RichTextParagraph(
                                    new RichTextWrapper(PlayerHero.PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                    new RichTextWrapper("触发"),
                                    new RichTextWrapper("强运", RichTextWrapper.GetColor(ColorEnum.Red)),
                                    new RichTextWrapper("。")
                                ));
                            await PlayerHero.PlayerContext.Player.PickCard(2);
                        }
                        await Task.FromResult(0);
                    }));
            await Task.FromResult(0);
        }

        bool ShouldTrigger()
        {
            return !PlayerHero.PlayerContext.Player.CardsInHand.Any();
        }
    }
}
