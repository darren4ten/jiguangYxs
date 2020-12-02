using System.Threading.Tasks;

namespace Logic.Model.Cards.Interface
{
    public interface IEquipment
    {
        Task Equip();

        Task UnEquip();
        bool IsViewableInSkillPanel();
    }
}
