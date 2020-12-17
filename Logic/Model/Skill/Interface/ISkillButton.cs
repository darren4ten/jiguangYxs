using System;
using System.Collections.Generic;
using System.Text;
using Logic.Event;
using Logic.Model.Enums;

namespace Logic.Model.Skill.Interface
{
    /// <summary>
    /// 技能区域的按钮,实现了该接口就代表要再技能按钮面板中显示按钮
    /// </summary>
    public interface ISkillButton
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        /// <returns></returns>
        bool IsEnabled();

        /// <summary>
        /// 获取按钮信息
        /// </summary>
        /// <returns></returns>
        SkillButtonInfo GetButtonInfo();
    }

    public class SkillButtonInfo
    {
        public SkillTypeEnum SkillType { get; set; }

        /// <summary>
        /// 技能按钮文字
        /// </summary>
        public string Text { get; set; }

        public string Description { get; set; }

        public EventBus.RoundEventHandler OnClick { get; set; }
    }
}
