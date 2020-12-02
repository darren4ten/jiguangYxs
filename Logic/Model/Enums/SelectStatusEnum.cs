using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Enums
{
    public enum SelectStatusEnum
    {
        /// <summary>
        /// 正常状态
        /// </summary>
        Idle,
        /// <summary>
        /// 被选中
        /// </summary>
        IsSelected,
        /// <summary>
        /// 不可被选中
        /// </summary>
        IsNotAbleToSelected,
        /// <summary>
        /// 等待选中
        /// </summary>
        PendingSelected
    }
}
