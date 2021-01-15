using Logic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Model.Cards.Interface;
using Logic.Model.Interface;
using Logic.Model.Enums;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 装备的基类
    /// </summary>
    public class EquipmentBase : CardBase, IEquipment, IAbility
    {
        public AttackDynamicFactor BaseAttackFactor { get; set; }

        public EquipmentBase()
        {
            BaseAttackFactor = new AttackDynamicFactor()
            {
                Damage = new Damage()
            };
        }

        public override async Task<CardResponseContext> PlayCard(CardRequestContext cardRequestContext, RoundContext roundContext)
        {
            //默认SrcPlayer为当前出牌的人
            cardRequestContext.SrcPlayer = cardRequestContext.SrcPlayer ?? PlayerContext.Player;
            Console.WriteLine($"[{cardRequestContext.SrcPlayer.PlayerName}{cardRequestContext.SrcPlayer.PlayerId}]的【{cardRequestContext.SrcPlayer.CurrentPlayerHero.Hero.DisplayName}】装备了【{ToString()}】");

            CardResponseContext responseContext = new CardResponseContext();
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforeZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforeEquip, cardRequestContext, responseContext, roundContext);
            if (responseContext.ResponseResult != ResponseResultEnum.UnKnown) return responseContext;

            var r1 = await OnBeforePlayCard(cardRequestContext, responseContext, roundContext);
            if (r1.ResponseResult != ResponseResultEnum.UnKnown) return r1;

            await PlayerContext.Player.TriggerEvent(EventTypeEnum.ZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.Equip, cardRequestContext, responseContext, roundContext);
            //装备技能
            await this.PlayerContext.Player.AddEquipment(this);
            //装备不会放入牌堆
            var isRemoved = PlayerContext.Player.CardsInHand.Remove(this);

            var r2 = await OnPlayCard(cardRequestContext, r1, roundContext);

            var r3 = await OnAfterPlayCard(cardRequestContext, r2, roundContext);
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterEquip, cardRequestContext, responseContext, roundContext);

            return r3;
        }

        public override bool CanBePlayed()
        {
            return PlayerContext.Player.IsInBeidongMode() || PlayerContext.Player.IsInZhudongMode();
        }
        public async Task Equip()
        {
            await OnBeforeEquip();
            await OnEquip();
            await OnAfterEquip();
        }

        public async Task UnEquip()
        {
            await OnBeforeUnEquip();
            await OnUnEquip();
            await OnAfterUnEquip();
        }

        public bool IsViewableInSkillPanel()
        {
            return false;
        }

        public virtual bool CanProvideSha()
        {
            return false;
        }

        public bool CanProvideFuidichouxin()
        {
            return false;
        }

        public bool CanProvideJiedaosharen()
        {
            return false;
        }

        public virtual bool CanProvideShan()
        {
            return false;
        }

        public virtual bool CanProvideYao()
        {
            return false;
        }

        public virtual bool CanProviderWuxiekeji()
        {
            return false;
        }

        public virtual bool CanProvideJuedou()
        {
            return false;
        }

        public virtual bool CanProvideFenghuolangyan()
        {
            return false;
        }

        public virtual bool CanProvideWanjianqifa()
        {
            return false;
        }

        public virtual bool CanProvideTannangquwu()
        {
            return false;
        }

        public bool CanProvideWuzhongshengyou()
        {
            return false;
        }

        public bool CanProvideHudadiweilao()
        {
            return false;
        }

        public bool CanProvideXiuyangshengxi()
        {
            return false;
        }

        #region 保护方法


        protected virtual Task OnBeforeEquip()
        {
            return Task.FromResult(0);
        }

        protected virtual Task OnEquip()
        {
            return Task.FromResult(0);
        }

        protected virtual Task OnAfterEquip()
        {
            return Task.FromResult(0);
        }

        protected virtual Task OnBeforeUnEquip()
        {
            return Task.FromResult(0);
        }

        protected virtual Task OnUnEquip()
        {
            return Task.FromResult(0);
        }

        protected virtual Task OnAfterUnEquip()
        {
            return Task.FromResult(0);
        }


        #endregion
    }
}
