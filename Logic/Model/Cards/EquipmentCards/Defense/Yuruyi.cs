using System;
using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.EquipmentCards.Defense
{
    /// <summary>
    /// 玉如意
    /// </summary>
    public class Yuruyi : EquipmentBase, IDefender
    {
        public Yuruyi()
        {
            this.Description = "玉如意";
            this.Name = "Yuruyi";
            this.DisplayName = "玉如意";
            this.CardType = Logic.Enums.CardTypeEnum.Defender;
        }

        public override bool CanBePlayed()
        {
            return true;
        }
    }
}
