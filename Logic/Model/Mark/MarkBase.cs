using System.Collections.Generic;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Enums;

namespace Logic.Model.Mark
{
    public abstract class MarkBase
    {
        /// <summary>
        /// 标记ID，用来确认标记类型
        /// </summary>
        public string MarkTypeId { get; protected set; }

        public MarkTypeEnum MarkType { get; set; }

        public string Image { get; set; }

        public List<CardBase> Cards { get; set; }

        public PlayerContext PlayerContext { get; set; }

        public MarkStatusEnum MarkStatus { get; set; }

        /// <summary>
        /// 标记次数，默认为1，用于可以累加标记的Mark，如闸刀
        /// </summary>
        public int MarkTimes { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 是否是负面标记
        /// </summary>
        /// <returns>true-是，false-不是，null:未知</returns>
        public virtual bool? IsNegativeMark()
        {
            return null;
        }

        /// <summary>
        /// 标记是否可以累加，默认false
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSummable()
        {
            return false;
        }

        /// <summary>
        /// 初始化。可以用来进行事件绑定
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// 重置Mark，取消事件绑定
        /// </summary>
        public abstract void Reset();

        public MarkBase()
        {
            MarkTimes = 1;
            MarkStatus = MarkStatusEnum.NotStarted;
            MarkTypeId = this.GetType().FullName;
        }
    }
}
