using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Model.RequestResponse.Response
{
    public class SelectiblityCheckResponse : BaseResponse
    {
        /// <summary>
        /// 是否可以被选中
        /// </summary>
        public bool CanBeSelected { get; set; }

        public string Message { get; set; }
    }
}
