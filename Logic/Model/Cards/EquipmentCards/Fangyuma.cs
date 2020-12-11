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
            BaseAttackFactor.DefenseDistance = 1;
        }

        protected override async Task OnEquip()
        {
            PlayerContext.Player.GetCurrentPlayerHero().BaseAttackFactor.DefenseDistance +=
                BaseAttackFactor.DefenseDistance;
            await Task.FromResult(0);
        }

        protected override async Task OnUnEquip()
        {
            PlayerContext.Player.GetCurrentPlayerHero().BaseAttackFactor.DefenseDistance -=
                BaseAttackFactor.DefenseDistance;
            await Task.FromResult(0);
        }
    }
}
