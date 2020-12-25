using System;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.JinlangCards
{
    /// <summary>
    /// 休养生息
    /// </summary>
    public class Xiuyangshengxi : JinnangBase, IGroupJinnang
    {
        public Xiuyangshengxi()
        {
            this.Description = "休养生息";
            this.Name = "Xiuyangshengxi";
            this.DisplayName = "休养生息";
        }

        public override bool CanBePlayed()
        {
            //打出条件
            //1. 被动出牌
            //2. 或者主动出牌且当前回合有player的血量不满
            return true;
        }

        public override Task Popup()
        {
            throw new NotImplementedException();
        }
    }
}
