using System.Threading.Tasks;
using Logic.Model.Enums;
using Logic.Model.Player;

namespace Logic.Model.Skill.Interface
{
    /// <summary>
    /// 能够增强杀
    /// </summary>
    public interface IEnhanceSha
    {
        /// <summary>
        /// 优先级，用来判断多个增强技能的处理顺序
        /// </summary>
        /// <returns></returns>
        int EnhancePriority();
    }
}
