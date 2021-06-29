using Logic.Model.Interface.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Log;
using Logic.Model.Skill.Zhudong;

namespace Logic.Model.Skill.Beidong
{
    /// <summary>
    /// 豹子头
    /// </summary>
    public class BaotouSkill : BeidongSkillBase
    {
        public BaotouSkill()
        {
            Name = "Baotou";
            this.DisplayName = "豹子头";
            Description = "出牌时，当出现以下两种情况时你对别的角色出的【杀】不可回避：（1）目标角色的手牌数大于或等于你的血量值。（2）目标角色的手牌数小于或等于你的攻击范围。";
        }

        public override async Task SetupEventListeners()
        {
            PlayerHero.PlayerContext.GameLevel.GlobalEventBus.
                ListenEvent(Guid.NewGuid(), PlayerHero, Logic.Model.Enums.EventTypeEnum.BeforeSha, (
                    async (reqContext, roundContext, responseContext) =>
                    {
                        if (ShouldTrigger(reqContext))
                        {
                            Console.WriteLine("触发豹子头");
                            PlayerHero.PlayerContext.GameLevel.LogManager.LogAction(
                                new RichTextParagraph(
                                    new RichTextWrapper(PlayerHero.PlayerContext.Player.ToString(), RichTextWrapper.GetColor(ColorEnum.Blue)),
                                    new RichTextWrapper("触发"),
                                    new RichTextWrapper("豹子头", RichTextWrapper.GetColor(ColorEnum.Red)),
                                    new RichTextWrapper("。")
                                ));
                            reqContext.AttackDynamicFactor.IsShaNotAvoidable = true;
                        }
                        await Task.FromResult(0);
                    }));
            await Task.FromResult(0);
        }

        bool ShouldTrigger(CardRequestContext request)
        {
            var target = request.TargetPlayers.First();
            return target.CardsInHand.Count >= PlayerHero.PlayerContext.Player.CurrentPlayerHero.CurrentLife || target.CardsInHand.Count <= PlayerHero.GetAttackFactor().ShaDistance;
        }
    }
}
