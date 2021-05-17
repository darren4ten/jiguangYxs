using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model.Player;
using Logic.Model.RequestResponse;

namespace Logic.GameLevel
{
    public class SelectTargetResponse : BaseResponse
    {
        public List<Player> Players { get; set; }
    }
}
