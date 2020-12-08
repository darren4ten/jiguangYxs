using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Model.Enums;
using Logic.Model.RequestResponse;
using Logic.Model.RequestResponse.Request;
using Logic.Model.RequestResponse.Response;

namespace Logic.ActionManger
{
    public class StandardActionManager : ActionManagerBase
    {
        public StandardActionManager(PlayerContext playContext) : base(playContext)
        {
        }

        public override async Task<bool> OnRequestTriggerSkill(SkillTypeEnum skillType, CardRequestContext cardRequestContext)
        {
            throw new NotImplementedException();
        }

        public override async Task<CardResponseContext> OnRequestResponseCard(CardRequestContext cardRequestContext)
        {
            throw new NotImplementedException();
        }

        public override async Task<CardResponseContext> OnRequestPickCardFromPanel(PickCardFromPanelRequest request)
        {
            throw new NotImplementedException();
        }

        public override async Task OnRequestStartStep_EnterMyRound()
        {
            throw new NotImplementedException();
        }

        public override async Task OnRequestStartStep_PickCard()
        {
            throw new NotImplementedException();
        }

        public override async Task OnRequestStartStep_PlayCard()
        {
            throw new NotImplementedException();
        }

        public override async Task OnRequestStartStep_ThrowCard()
        {
            throw new NotImplementedException();
        }

        public override async Task OnRequestStartStep_ExitMyRound()
        {
            throw new NotImplementedException();
        }

        public override async Task<SelectedTargetsResponse> OnRequestSelectTargets(SelectedTargetsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
