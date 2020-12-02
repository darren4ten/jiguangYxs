using System.Threading.Tasks;
using Logic.Model.Enums;

namespace Logic.Model.Skill.Interface
{
    public interface ISkill
    {
        /// <summary>
        /// 装备技能
        /// </summary>
        /// <returns></returns>
        Task LoadSkill();

        /// <summary>
        /// 卸载技能
        /// </summary>
        /// <returns></returns>
        Task UnLoadSkill();

        /// <summary>
        /// 技能类型
        /// </summary>
        /// <returns></returns>
        SkillTypeEnum SkillType();
    }
}
