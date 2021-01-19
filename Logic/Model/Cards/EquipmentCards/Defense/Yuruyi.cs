using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Logic.Log;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;

namespace Logic.Model.Cards.EquipmentCards.Defense
{
    /// <summary>
    /// 玉如意
    /// </summary>
    public class Yuruyi : EquipmentBase, IDefender
    {
        private Guid eventId;
        public Yuruyi()
        {
            this.Description = "玉如意";
            this.Name = "Yuruyi";
            this.DisplayName = "玉如意";
            this.Image = "/Resources/card/equipment/card_yuruyi.jpg";
        }

        protected override async Task OnEquip()
        {
            //监听BeforeBeRequestedShan事件，
            this.eventId = Guid.NewGuid();
            PlayerContext.Player.ListenEvent(eventId, Enums.EventTypeEnum.BeidongPlayCard, (
                async (context, roundContext, responseContext) =>
                {
                    if (!(context.RequestCard is Shan))
                    {
                        return;
                    }
                    var target = context.TargetPlayers.FirstOrDefault();
                    if (target == null)
                    {
                        throw new Exception("被杀目标不能为空");
                    }

                    if (!context.AttackDynamicFactor.IsShaNotAvoidableByYuruyi)
                    {
                        var shouldTrigger = await PlayerContext.Player.ActionManager.OnRequestTriggerSkill(SkillTypeEnum.Yuruyi, context);
                        if (shouldTrigger)
                        {
                            Console.WriteLine($"{PlayerContext.Player.PlayerId}【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】发动玉如意判定。");

                            PlayerContext.GameLevel.LogManager.LogAction(
                              new RichTextParagraph(
                               new RichTextWrapper($"{PlayerContext.Player.PlayerId}【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue), 12, true),
                               new RichTextWrapper("发动玉如意"),
                               new RichTextWrapper(ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                               new RichTextWrapper("判定")
                           ));

                            var pandingRes = await PlayerContext.GameLevel.Panding(context, (c) => c.Color == CardColorEnum.Red);
                            if (pandingRes.ResponseResult == ResponseResultEnum.Success)
                            {
                                Console.WriteLine($"判定成功，判定牌为【{pandingRes.Cards.FirstOrDefault()}】");
                                PlayerContext.GameLevel.LogManager.LogAction(
                                    new RichTextParagraph(
                                     new RichTextWrapper("判定"),
                                     new RichTextWrapper("成功", RichTextWrapper.GetColor(ColorEnum.Red)),
                                     new RichTextWrapper(",判定牌为"),
                                     new RichTextWrapper(pandingRes.Cards.FirstOrDefault()?.ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                     new RichTextWrapper("。")
                                 ));
                                responseContext.ResponseResult = ResponseResultEnum.Success;
                                responseContext.Message = $"玉如意判定成功，判定牌为：【{pandingRes.Cards.FirstOrDefault()}】";
                                return;
                            }
                            else if (pandingRes.ResponseResult == ResponseResultEnum.Cancelled)
                            {
                                PlayerContext.GameLevel.LogManager.LogAction(
                                     new RichTextParagraph(
                                      new RichTextWrapper("判定"),
                                      new RichTextWrapper("取消", RichTextWrapper.GetColor(ColorEnum.Red)),
                                      new RichTextWrapper("。")
                                  ));
                                Console.WriteLine($"判定取消");
                            }
                            else
                            {
                                PlayerContext.GameLevel.LogManager.LogAction(
                                     new RichTextParagraph(
                                      new RichTextWrapper("判定"),
                                      new RichTextWrapper("失败", RichTextWrapper.GetColor(ColorEnum.Red)),
                                     new RichTextWrapper(",判定牌为"),
                                     new RichTextWrapper(pandingRes.Cards.FirstOrDefault()?.ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                                     new RichTextWrapper("。")
                                  ));
                                Console.WriteLine($"判定失败，判定牌为【{pandingRes.Cards.FirstOrDefault()}】");
                            }
                        }
                    }
                }));
            await Task.FromResult(0);
        }

        protected override async Task OnUnEquip()
        {
            //注销监听事件
            PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.BeidongPlayCard, eventId);
            await Task.FromResult(0);
        }
    }
}
