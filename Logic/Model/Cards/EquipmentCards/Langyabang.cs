using System;
using System.Linq;
using System.Threading.Tasks;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 狼牙棒。最后一张牌是杀时可以选择最多3个目标
    /// </summary>
    public class Langyabang : EquipmentBase, IWeapon
    {
        private Guid eventId;
        public Langyabang()
        {
            this.Description = "狼牙棒";
            this.Name = "Langyabang";
            this.DisplayName = "狼牙棒";
            BaseAttackFactor.ShaDistance = 4;
        }
        protected override async Task OnEquip()
        {
            PlayerContext.Player.GetCurrentPlayerHero().BaseAttackFactor.ShaDistance += BaseAttackFactor.ShaDistance - 1;
            //监听选择目标事件
            //触发条件：
            //  1. 选择目标的请求是杀
            //  2. 且最后一张手牌是杀
            eventId = Guid.NewGuid();
            PlayerContext.Player.ListenEvent(eventId, Enums.EventTypeEnum.BeforeSelectTarget, (
                async (context, roundContext, responseContext) =>
                {
                    if (context.AttackType == Enums.AttackTypeEnum.Sha
                        && PlayerContext.Player.CardsInHand != null
                        && PlayerContext.Player.CardsInHand.Count == 1
                        && PlayerContext.Player.CardsInHand.First() is Sha)
                    {
                        context.AttackDynamicFactor.MaxShaTargetCount = 3;
                    }

                    await Task.FromResult(0);
                }));

            await Task.FromResult(0);
        }

        protected override async Task OnUnEquip()
        {
            PlayerContext.Player.GetCurrentPlayerHero().BaseAttackFactor.ShaDistance -= BaseAttackFactor.ShaDistance - 1;
            PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.BeforeSelectTarget, eventId);
            await Task.FromResult(0);
        }
    }
}
