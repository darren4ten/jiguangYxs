using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Enums;

namespace Logic.GameLevel
{
    /// <summary>
    /// 请求出牌的上下文
    /// </summary>
    public class RequestContext
    {
        public RequestCardTypeEnum RequestCardType { get; set; }

        public int MaxCount { get; set; }

        public int MinCount { get; set; }

        //public void Notifiy();
    }
}
