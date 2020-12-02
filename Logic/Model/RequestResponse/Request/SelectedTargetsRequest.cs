using Logic.GameLevel;
using Logic.Model.Enums;

namespace Logic.Model.RequestResponse.Request
{
    /// <summary>
    /// 选择目标
    /// </summary>
    public class SelectedTargetsRequest : BaseRequest
    {
        /// <summary>
        /// 选择目标的类型
        /// </summary>
        public SelectTargetTypeEnum TargetType { get; set; }

        public CardRequestContext CardRequest { get; set; }
        public RoundContext RoundContext { get; set; }
    }
}
