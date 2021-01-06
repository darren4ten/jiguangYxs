using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Cards.Interface;
using Logic.Model.Enums;
using Logic.Model.Mark;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 手捧雷
    /// </summary>
    public class Shoupenglei : JinnangBase, IDelayJinnang
    {
        public Shoupenglei()
        {
            this.Description = "手捧雷";
            this.Name = "Shoupenglei";
            this.DisplayName = "手捧雷";
        }

        public override bool CanBePlayed()
        {
            return base.CanBePlayed() && PlayerContext.Player.Marks?.Any(m => m is ShoupengleiMark) != true;
        }

        protected override async Task<CardResponseContext> OnAfterPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            //打出牌之后，创建手捧雷标记并将标记转移给目标
            var mark = new ShoupengleiMark()
            {
                MarkType = MarkTypeEnum.Card,
                MarkStatus = MarkStatusEnum.NotStarted,
                Cards = new List<CardBase>() { this },
            };
            await PlayerContext.Player.AddMark(mark);
            return cardResponseContext;
        }
    }
}
