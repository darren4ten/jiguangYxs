using System.Threading.Tasks;

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
            this.Image = "/Resources/card/equipment/card_fangyuma.jpg";
            BaseAttackFactor.DefenseDistance = 1;
        }

        protected override async Task OnEquip()
        {
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.DefenseDistance +=
                BaseAttackFactor.DefenseDistance;
            await Task.FromResult(0);
        }

        protected override async Task OnUnEquip()
        {
            PlayerContext.Player.CurrentPlayerHero.BaseAttackFactor.DefenseDistance -=
                BaseAttackFactor.DefenseDistance;
            await Task.FromResult(0);
        }
    }
}
