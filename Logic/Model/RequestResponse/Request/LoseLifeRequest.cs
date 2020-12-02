using Logic.GameLevel;
using Logic.Model.Enums;

namespace Logic.Model.RequestResponse.Request
{
    public class LoseLifeRequest : BaseRequest
    {
        public DamageTypeEnum DamageType { get; set; }

        public CardRequestContext CardRequestContext { get; set; }
        public CardResponseContext CardResponseContext { get; set; }

        public RoundContext SrcRoundContext { get; set; }
    }
}
