using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 借刀杀人
    /// </summary>
    public class Jiedaosharen : JinnangBase, INeedTargets
    {
        public Jiedaosharen()
        {
            this.Description = "借刀杀人";
            this.Name = "Jiedaosharen";
            this.DisplayName = "借刀杀人";
        }

        public override bool CanBePlayed()
        {
            //能出杀的条件：
            //1. 被动出牌
            //2. 主动出牌，且场上有英雄装备武器（不保证是真的能借，如诸葛亮不能被黑色牌所借）
            return (PlayerContext.Player.IsInZhudongMode() && PlayerContext.GameLevel.Players.Any(p => p.IsAlive && p.EquipmentSet.Any(e => e is IWeapon))) || PlayerContext.Player.IsInBeidongMode();
        }

        public SelectedTargetsRequest GetSelectTargetRequest()
        {
            return new SelectedTargetsRequest()
            {
                MinTargetCount = 1,
                MaxTargetCount = 1,
                CardRequest = CardRequestContext.GetBaseCardRequestContext(null),
                RoundContext = PlayerContext.Player.RoundContext,
                TargetType = AttackTypeEnum.Jiedaosharen
            };
        }

        protected override async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            cardRequestContext.AttackType = Enums.AttackTypeEnum.Jiedaosharen;
            return await Task.FromResult(cardResponseContext);
        }

        protected override async Task<CardResponseContext> OnPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            return await ExecuteAction(cardRequestContext, cardResponseContext, roundContext);
        }

        private async Task<CardResponseContext> ExecuteAction(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            if (cardRequestContext.TargetPlayers.Count != 2)
            {
                cardResponseContext.ResponseResult = Enums.ResponseResultEnum.Failed;
                cardResponseContext.Message = "必须选择攻击目标。";
                return cardResponseContext;
            }

            var from = cardRequestContext.TargetPlayers[0];
            var to = cardRequestContext.TargetPlayers[1];
            var cardRes = await from.ResponseCard(new CardRequestContext()
            {
                RequestCard = new Sha(),
                AttackType = AttackTypeEnum.Jiedaosharen,
                TargetPlayers = new List<Player.Player>() { to }
            }, cardResponseContext, roundContext);
            //如果出了杀，则发动杀
            if (cardRes.Cards != null && cardRes.Cards.Any())
            {
                var sha = cardRes.Cards.First();
                await sha.PlayCard(CardRequestContext.GetBaseCardRequestContext(new List<Player.Player>() { to }), null);
            }
            else
            {
                var weapon = from.EquipmentSet.FirstOrDefault(p => p is IWeapon);
                //没有杀则获得from的武器
                await from.MoveCardToTargetHand(to, new List<CardBase>()
                {
                    weapon
                });
            }
            return cardResponseContext;
        }
    }
}
