using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.Mark
{
    /// <summary>
    /// 毒计标记
    /// </summary>
    public class DujiMark : MarkBase
    {
        public DujiMark()
        {
            MarkStatus = Enums.MarkStatusEnum.NotStarted;
            MarkType = Enums.MarkTypeEnum.Mark;
            Description = "毒计";
            Image = "/Resources/card/card_sha.jpg";
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
