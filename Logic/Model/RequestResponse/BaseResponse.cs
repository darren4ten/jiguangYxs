using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Enums;

namespace Logic.Model.RequestResponse
{
    public class BaseResponse
    {
        /// <summary>
        /// 响应状态
        /// </summary>
        public ResponseResultEnum Status { get; set; }
    }
}
