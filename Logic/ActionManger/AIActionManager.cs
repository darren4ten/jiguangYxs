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
    /// <summary>
    /// AI行为管理器 
    /// </summary>
    public class AIActionManager : IActionManager
    {
        public Task<bool> OnRequestTriggerSkill(SkillTypeEnum skillType, CardRequestContext cardRequestContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 请求出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        public Task<CardResponseContext> OnRequestResponseCard(CardRequestContext cardRequestContext)
        {
            throw new NotImplementedException();
        }

        public Task<CardResponseContext> OnRequestPickCardFromPanel(PickCardFromPanelRequest request)
        {
            throw new NotImplementedException();
        }

        public Task OnRequestStartStep_EnterMyRound()
        {
            throw new NotImplementedException();
        }

        public Task OnRequestStartStep_PickCard()
        {
            throw new NotImplementedException();
        }

        public Task OnRequestStartStep_PlayCard()
        {
            throw new NotImplementedException();
        }

        public Task OnRequestStartStep_ThrowCard()
        {
            throw new NotImplementedException();
        }

        public Task OnRequestStartStep_ExitMyRound()
        {
            throw new NotImplementedException();
        }

        public Task<SelectedTargetsResponse> OnRequestSelectTargets(SelectedTargetsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
