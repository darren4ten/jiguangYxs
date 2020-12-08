using Logic.Model.Cards.Interface;

namespace Logic.Model.Cards.EquipmentCards
{
    /// <summary>
    /// 博浪锤
    /// </summary>
    public class Bolangchui : EquipmentBase, IWeapon
    {
        public Bolangchui()
        {
            this.Description = "博浪锤";
            this.Name = "Bolangchui";
            this.DisplayName = "博浪锤";
            BaseAttackFactor.ShaDistance = 3;
        }

        public override bool CanBePlayed()
        {
            return true;
        }
    }
}
