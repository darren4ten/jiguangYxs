using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Mark
{
    /// <summary>
    /// 手捧雷标记
    /// </summary>
    public class ShoupengleiMark : MarkBase
    {
        public ShoupengleiMark()
        {
            MarkStatus = Enums.MarkStatusEnum.NotStarted;
            MarkType = Enums.MarkTypeEnum.Card;
            Description = "判定黑桃2-9则爆炸";
        }

        public override void Init()
        {
            //todo:事件绑定
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
