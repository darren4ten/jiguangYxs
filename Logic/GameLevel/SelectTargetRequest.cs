using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.RequestResponse;

namespace Logic.GameLevel
{
    /// <summary>
    /// 选择目标的请求
    /// </summary>
    public class SelectTargetRequest : BaseRequest
    {
        public int MaxCount { get; set; }

        public int MinCount { get; set; }
    }
}
