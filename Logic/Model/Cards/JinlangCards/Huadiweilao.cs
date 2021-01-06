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
    /// 画地为牢
    /// </summary>
    public class Huadiweilao : JinnangBase, IDelayJinnang
    {
        public Huadiweilao()
        {
            this.Description = "画地为牢";
            this.Name = "Huadiweilao";
            this.DisplayName = "画地为牢";
        }

        /// <summary>
        /// 能否出牌
        /// </summary>
        /// <returns></returns>
        public override bool CanBePlayed()
        {
            //主动情况下，如果有可选的目标（玩家英雄能被选中（如：控局），且改目标没有画地为牢标记）
            return base.CanBePlayed();
        }

        protected override async Task<CardResponseContext> OnAfterPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext,
            RoundContext roundContext)
        {
            var target = cardRequestContext.TargetPlayers.FirstOrDefault();
            //打出牌之后，创建画地为牢标记并将标记转移给目标
            var mark = new HuadiweilaoMark()
            {
                MarkType = MarkTypeEnum.Card,
                MarkStatus = MarkStatusEnum.NotStarted,
                Cards = new List<CardBase>() { this },
            };
            await target.AddMark(mark);
            return cardResponseContext;
        }
    }
}
