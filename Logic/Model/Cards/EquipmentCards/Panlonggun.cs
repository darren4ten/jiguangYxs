using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Enums;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 盘龙棍
    /// </summary>
    public class Panlonggun : EquipmentBase, IWeapon
    {
        private Guid eventId;
        public Panlonggun()
        {
            this.Description = "盘龙棍";
            this.Name = "Panlonggun";
            this.DisplayName = "盘龙棍";
            BaseAttackFactor.ShaDistance = 3;
        }

        protected override async Task OnEquip()
        {
            //增加攻击距离
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.ShaDistance +=
                BaseAttackFactor.ShaDistance - 1;
            await Task.FromResult(0);
            //监听杀失败事件，如果杀失败(Failed,不是Cancelled)，询问用户是否继续出杀
            this.eventId = Guid.NewGuid();
            PlayerContext.Player.ListenEvent(eventId, Enums.EventTypeEnum.AfterShaFailed, (
               async (context, roundContext, responseContext) =>
               {
                   var target = context.TargetPlayers.FirstOrDefault();
                   if (target == null)
                   {
                       throw new Exception("被杀目标不能为空");
                   }


                   var shuouldTrigger = await PlayerContext.Player.ActionManager.OnRequestTriggerSkill(SkillTypeEnum.Panlonggun, context);
                   if (shuouldTrigger)
                   {
                       var res = await PlayerContext.Player.ResponseCard(new CardRequestContext()
                       {
                           CardScope = CardScopeEnum.InHand,
                           AttackType = AttackTypeEnum.Bolangchui,
                           MaxCardCountToPlay = 1,
                           MinCardCountToPlay = 1,
                           RequestCard = new Sha(),
                           SrcPlayer = PlayerContext.Player,
                           TargetPlayers = new List<Player.Player>() { PlayerContext.Player }
                       }, responseContext, roundContext);
                       if (res.ResponseResult == ResponseResultEnum.Success || (res.Cards != null && res.Cards.Any()))
                       {
                           var newSha = res.Cards.FirstOrDefault();
                           if (newSha == null)
                           {
                               throw new Exception("必须出杀才能发动盘龙棍");
                           }
                           Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】发动盘龙棍技能打出【{newSha}】");
                           var shaRes = await newSha.PlayCard(CardRequestContext.GetBaseCardRequestContext(context.TargetPlayers), roundContext);
                       }
                       else
                       {
                           Console.WriteLine($"{PlayerContext.Player.PlayerId}的【{PlayerContext.Player.CurrentPlayerHero.Hero.DisplayName}】放弃发动盘龙棍技能】");
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
