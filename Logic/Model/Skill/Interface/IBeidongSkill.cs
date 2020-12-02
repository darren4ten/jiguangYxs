using System.Threading.Tasks;

namespace Logic.Model.Skill.Interface
{
    public interface IBeidongSkill : ISkill
    {
        /// <summary>
        ///  设置事件
        /// </summary>
        /// <returns></returns>
        Task SetupEventListeners();
    }
}
