using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Logic.Annotations;
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

    public class SkillButtonInfo : INotifyPropertyChanged
    {
        public SkillTypeEnum SkillType { get; set; }

        /// <summary>
        /// 技能按钮文字
        /// </summary>
        public string Text { get; set; }


        public bool IsEnabled => BtnEnableCheck != null && BtnEnableCheck();
        

        public string Description { get; set; }

        public EventBus.RoundEventHandler OnClick { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 检查按钮是否可用
        /// </summary>
        public Func<bool> BtnEnableCheck { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
