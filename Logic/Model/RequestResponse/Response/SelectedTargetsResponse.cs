using Logic.Model.Player;

namespace Logic.Model.RequestResponse.Response
{
    /// <summary>
    /// 选择目标的响应
    /// </summary>
    public class SelectedTargetsResponse : BaseResponse
    {
        /// <summary>
        /// 第一个
        /// </summary>
        public PlayerHero FirstPlayerHero { get; set; }
        public PlayerHero SecondPlayerHero { get; set; }
        public PlayerHero ThirdPlayerHero { get; set; }
    }
}
