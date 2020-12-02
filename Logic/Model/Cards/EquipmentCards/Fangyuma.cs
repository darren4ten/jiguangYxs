namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 防御马
    /// </summary>
    public class Fangyuma : EquipmentBase
    {
        public Fangyuma()
        {
            this.Description = "防御马";
            this.Name = "Fangyuma";
            this.DisplayName = "防御马";
            this.CardType = Logic.Enums.CardTypeEnum.Defender;
            BaseAttackFactor.ShaDistance = 2;
            BaseAttackFactor.DefenseDistance = 1;
        }

        public override bool CanBePlayed()
        {
            return true;
        }
    }
}
