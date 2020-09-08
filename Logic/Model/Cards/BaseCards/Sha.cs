using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Cards.BaseCards
{
    public class Sha : CardBase
    {
        public Sha()
        {
            this.Description = "杀";
            this.Name = "Sha";
            this.DisplayName = "杀";
            this.CardType = Enums.CardTypeEnum.Base;
        }

        public override bool CanBePlayedFunc()
        {
            //case 1:(主动杀)
            //    IsMyRoud &&  max_num_sha > played_num_sha && player.attackDistance>=(enermy.postion-player.position)
            //case 2:(被动 杀,被请求出杀)
            //       2.1:(被决斗或者决斗)
            //       2.2:(烽火狼烟)
            //       2.3:(被借刀杀人)
            //       2.4:(被拼点)

            var canPlayZhudong = this.CardContext.FromPlayer.IsMyRound
                && this.CardContext.FromPlayer.UserHero.GetMaxShaCount() > this.CardContext.FromPlayer.UserHero.ShaedCount;
            var canPlayBeidong = this.CardContext.ToPlayer.IsMyTurn;

            return canPlayZhudong || canPlayBeidong;
        }

        public override void PlayCard()
        {
            base.PlayCard();
        }

        public override bool IsTargetSelectable(CardContext cardContext)
        {
            var playerDist = Math.Abs(cardContext.ToPlayer.PlayerIndex - cardContext.FromPlayer.PlayerIndex) + cardContext.ToPlayer.GetDefenseDistance();
            var can = (cardContext.FromPlayer.IsMyRound || cardContext.FromPlayer.IsMyTurn)
                && cardContext.FromPlayer.UserHero.GetMaxAttackDistance() >= playerDist;
            return can;
        }

        public override void TriggerResultFunc()
        {
            throw new NotImplementedException();
        }
    }
}
