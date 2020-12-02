using System.Threading.Tasks;

namespace Logic.Model.Skill.Interface
{
    public interface IZhudongSkill : ISkill
    {
        Task Trigger();

        /// <summary>
        /// 是否可以触发
        /// </summary>
        /// <returns></returns>
        bool IsTriggerable();
    }
}
