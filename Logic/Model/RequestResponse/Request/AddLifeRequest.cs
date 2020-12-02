using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Enums;

namespace Logic.Model.RequestResponse.Request
{
    public class AddLifeRequest : BaseRequest
    {
        public RecoverTypeEnum RecoverType { get; set; }

        /// <summary>
        /// 谁来触发的回血
        /// </summary>
        public Player.Player FromPlayer { get; set; }
    }
}
