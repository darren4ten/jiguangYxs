using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.GameLevel.Panel;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 博浪锤
    /// </summary>
    public class Bolangchui : EquipmentBase, IWeapon
    {
        private Guid eventId;
        public Bolangchui()
        {
            this.Description = "博浪锤";
            this.Name = "Bolangchui";
            this.DisplayName = "博浪锤";
            BaseAttackFactor.ShaDistance = 3;
        }

        protected override async Task OnEquip()
        {
            //增加攻击距离
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance +=
                BaseAttackFactor.ShaDistance - 1;
            await Task.FromResult(0);
            //监听杀失败事件，如果杀失败(Failed,不是Cancelled)，询问用户是否弃掉两张牌强制命中
            this.eventId = Guid.NewGuid();
            PlayerContext.Player.ListenEvent(eventId, Enums.EventTypeEnum.AfterShaFailed, (
               async (context, roundContext, responseContext) =>
               {
                   var target = context.TargetPlayers.FirstOrDefault();
                   if (target == null)
                   {
                       throw new Exception("被杀目标不能为空");
                   }

                   var shuouldTrigger = await PlayerContext.Player.ActionManager.OnRequestTriggerSkill(SkillTypeEnum.Bolangchui, context);
                   if (shuouldTrigger)
                   {
                       var res = await PlayerContext.Player.ResponseCard(new CardRequestContext()
                       {
                           CardScope = CardScopeEnum.InHandAndEquipment,
                           AttackType = AttackTypeEnum.Bolangchui,
                           MaxCardCountToPlay = 2,
                           MinCardCountToPlay = 2,
                           SrcPlayer = PlayerContext.Player,
                           TargetPlayers = new List<Player.Player>() { PlayerContext.Player }
                       }, responseContext, roundContext);
                       if (res.ResponseResult == ResponseResultEnum.Success || (res.Cards != null && res.Cards.Count == 2))
                       {
                           Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】弃掉两张牌{String.Join(",", res.Cards?.Select(p => p.ToString()) ?? new List<string>())}发动博浪锤技能强制命中【{target.PlayerId}{target.CurrentPlayerHero.Hero.DisplayName}】");
                           //弃掉对应的牌
                           res.Cards?.ForEach(async c =>
                           {
                               //是装备牌
                               if (PlayerContext.Player.EquipmentSet.Any(p => p == c))
                               {
                                   await PlayerContext.Player.RemoveEquipment(c, null, null, null);
                               }
                               else
                               {
                                   await PlayerContext.Player.RemoveCardsInHand(new List<CardBase>() { c }, null, null, null);
                               }
                           });
                           //强制命中
                           await target.CurrentPlayerHero.LoseLife(new LoseLifeRequest()
                           {
                               RequestId = context.RequestId,
                               CardRequestContext = context,
                               CardResponseContext = responseContext,
                               DamageType = DamageTypeEnum.Sha,
                               SrcRoundContext = PlayerContext.Player.RoundContext
                           });
                       }
                   }
               }));
        }

        protected override async Task OnUnEquip()
        {
            //扣除攻击距离
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance -=
                BaseAttackFactor.ShaDistance - 1;
            //注销监听事件
            PlayerContext.GameLevel.GlobalEventBus.RemoveEventListener(EventTypeEnum.AfterShaFailed, eventId);
            await Task.FromResult(0);
        }
    }
}
