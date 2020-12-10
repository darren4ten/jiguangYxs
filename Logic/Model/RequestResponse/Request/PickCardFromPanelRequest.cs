using System.Collections.Generic;
using Logic.Cards;
using Logic.GameLevel.Panel;
using Logic.Model.Player;

namespace Logic.Model.RequestResponse.Request
{
    public class PickCardFromPanelRequest : BaseRequest
    {
        public PanelBase Panel { get; set; }

        /// <summary>
        /// 取牌的最大数量
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// 取牌的最小数量
        /// </summary>
        public int MinCount { get; set; }
    }
}
