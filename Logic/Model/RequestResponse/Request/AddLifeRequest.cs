using System;
using System.Collections.Generic;
using System.Text;
using Logic.GameLevel;
using Logic.Model.Enums;

namespace Logic.Model.RequestResponse.Request
{
    public class AddLifeRequest : BaseRequest
    {
        public RecoverTypeEnum RecoverType { get; set; }

        public CardRequestContext CardRequestContext { get; set; }
        public CardResponseContext CardResponseContext { get; set; }

        public RoundContext SrcRoundContext { get; set; }
    }
}
