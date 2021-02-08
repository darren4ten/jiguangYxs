using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.Model.Enums;
using Logic.Model.RequestResponse;
using Logic.Model.RequestResponse.Request;
using Logic.Model.RequestResponse.Response;

namespace Logic.ActionManger
{
    public interface IActionManager
    {
        /// <summary>
        /// 被请求发动技能返回
        /// </summary>
        /// <param name="skillType"></param>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        Task<bool> OnRequestTriggerSkill(SkillTypeEnum skillType, CardRequestContext cardRequestContext);

        /// <summary>
        /// 并行被请求出牌（多个目标只需要一个目标出牌即可）
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        Task<CardResponseContext> OnParallelRequestResponseCard(CardRequestContext cardRequestContext);

        /// <summary>
        /// 被请求出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        Task<CardResponseContext> OnRequestResponseCard(CardRequestContext cardRequestContext);

        /// <summary>
        ///  被请求从面板中摸n张牌
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CardResponseContext> OnRequestPickCardFromPanel(PickCardFromPanelRequest request);

        /// <summary>
        /// 请求开始回合阶段
        /// </summary>
        /// <returns></returns>
        Task OnRequestStartStep_EnterMyRound();

        /// <summary>
        /// 请求开始出牌阶段
        /// </summary>
        /// <returns></returns>
        Task OnRequestStartStep_PickCard();

        /// <summary>
        /// 请求开始出牌阶段;
        /// </summary>
        /// <returns></returns>
        Task OnRequestStartStep_PlayCard();

        /// <summary>
        ///  请求开始弃牌阶段
        /// </summary>
        /// <param name="throwCount"></param>
        /// <returns></returns>
        Task<List<CardBase>> OnRequestStartStep_ThrowCard(int throwCount);

        /// <summary>
        /// 请求结束回合阶段
        /// </summary>
        /// <returns></returns>
        Task OnRequestStartStep_ExitMyRound();

        /// <summary>
        /// 被请求选择目标
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SelectedTargetsResponse> OnRequestSelectTargets(SelectedTargetsRequest request);
    }
}
