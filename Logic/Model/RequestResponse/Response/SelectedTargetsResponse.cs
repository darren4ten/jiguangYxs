using System.Collections.Generic;

namespace Logic.Model.RequestResponse.Response
{
    /// <summary>
    /// 选择目标的响应
    /// </summary>
    public class SelectedTargetsResponse : BaseResponse
    {
        public List<Player.Player> Targets { get; set; } = new List<Player.Player>();
    }
}
