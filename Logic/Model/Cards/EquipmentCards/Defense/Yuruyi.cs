using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
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
                            var pandingRes = await PlayerContext.GameLevel.Panding(context, (c) => c.Color == CardColorEnum.Red);
                            if (pandingRes.ResponseResult == ResponseResultEnum.Success)
                            {
                                Console.WriteLine($"判定成功，判定牌为【{pandingRes.Cards.FirstOrDefault()}】");
                                responseContext.ResponseResult = ResponseResultEnum.Success;
                                responseContext.Message = $"玉如意判定成功，判定牌为：【{pandingRes.Cards.FirstOrDefault()}】";
                                return;
                            }
                            else if (pandingRes.ResponseResult == ResponseResultEnum.Cancelled)
                            {
                                Console.WriteLine($"判定取消");
                            }
                            else
                            {
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
