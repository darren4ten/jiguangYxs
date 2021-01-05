using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Model.Enums;
using Logic.Model.RequestResponse.Request;
using Logic.Model.RequestResponse.Response;

namespace Logic.ActionManger
{
    public abstract class ActionManagerBase : IActionManager
    {
        public PlayerContext PlayerContext { get; private set; }
        public ActionManagerBase(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
        }

        public ActionManagerBase()
        {
        }

        public void SetPlayerContext(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
        }

        public abstract Task<bool> OnRequestTriggerSkill(SkillTypeEnum skillType,
            CardRequestContext cardRequestContext);

        public abstract Task<CardResponseContext> OnParallelRequestResponseCard(CardRequestContext cardRequestContext);
      

        public abstract Task<CardResponseContext> OnRequestResponseCard(CardRequestContext cardRequestContext);

        public abstract Task<CardResponseContext> OnRequestPickCardFromPanel(PickCardFromPanelRequest request);

        public abstract Task OnRequestStartStep_EnterMyRound();

        public abstract Task OnRequestStartStep_PickCard();

        public abstract Task OnRequestStartStep_PlayCard();

        public abstract Task OnRequestStartStep_ThrowCard();

        public abstract Task OnRequestStartStep_ExitMyRound();

        public abstract Task<SelectedTargetsResponse> OnRequestSelectTargets(SelectedTargetsRequest request);
    }
}
