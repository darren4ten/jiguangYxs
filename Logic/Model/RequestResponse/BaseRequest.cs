using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;

namespace Logic.Model.RequestResponse
{
    public class BaseRequest
    {
        public Guid? RequestId { get; set; }


        public TaskCompletionSource<CardResponseContext> RequestTaskCompletionSource { get; set; }
    }
}
