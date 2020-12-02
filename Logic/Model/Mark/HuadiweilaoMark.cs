using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Mark
{
    /// <summary>
    /// 画地为牢标记
    /// </summary>
    public class HuadiweilaoMark : MarkBase
    {
        public HuadiweilaoMark()
        {
            MarkStatus = Enums.MarkStatusEnum.NotStarted;
            MarkType = Enums.MarkTypeEnum.Card;
            Description = "画地为牢";
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
