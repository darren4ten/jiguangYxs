using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.JinlangCards;
using Logic.Model.Enums;
using Logic.Model.Interface;
using Logic.Model.Player;
using Logic.Model.RequestResponse;
using Logic.Model.RequestResponse.Request;
using Logic.Model.RequestResponse.Response;
using Logic.Util;

namespace Logic.ActionManger
{
    /// <summary>
    /// 标准用户行为管理器，即人工处理器
    /// </summary>
    public class StandardActionManager : ActionManagerBase
    {
        public StandardActionManager(PlayerContext playContext) : base(playContext)
        {
        }

        public override async Task<bool> OnRequestTriggerSkill(SkillTypeEnum skillType, CardRequestContext cardRequestContext)
        {
            //询问是否发动技能，一般只有被动技能才会需要询问，比如：集权、反击、鲁莽
            //提示技能消息
            //弹出确认、取消按钮
            //如果点击确认或者取消则返回true或者false

            string displayMessage = $"是否发动“{skillType.GetDescription()}?”";
            var tcs = new TaskCompletionSource<CardResponseContext>();
            PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(tcs, displayMessage);
            var res = await tcs.Task;
            return res != null && res.ResponseResult == ResponseResultEnum.Success ? true : false;
        }

        public override async Task<CardResponseContext> OnParallelRequestResponseCard(CardRequestContext cardRequestContext)
        {
            //1. 手牌中是否有请求的牌
            //2. 技能是否能够提供请求的牌
            if (PlayerContext.Player.CardsInHand.All(c => c.GetType() != cardRequestContext.RequestCard.GetType()))
            {
                var tcs = new TaskCompletionSource<CardResponseContext>();

                Func<Task<CardResponseContext>> showMsg = async () =>
                {
                    string displayMessage = $"是否打出“{cardRequestContext.RequestCard.DisplayName}?”";
                    //设置被动请求，包括
                    //设置被动请求参数、
                    //UI弹出提示信息、
                    //等待玩家选择卡牌（检查CardRequestContext）,如果IsGroupRequest=true则代表不弃牌，卡牌弹出并确认按钮之后tcs.setresult
                    PlayerContext.Player.SetCardRequestContext(new CardRequestContext()
                    {
                        Message = displayMessage,
                        RequestCard = cardRequestContext.RequestCard,
                        SrcPlayer = cardRequestContext.SrcPlayer,
                        MaxCardCountToPlay = cardRequestContext.MaxCardCountToPlay,
                        MinCardCountToPlay = cardRequestContext.MaxCardCountToPlay,
                        IsGroupRequest = true,
                        RequestTaskCompletionSource = tcs
                    });
                    var res = await tcs.Task;
                    return res;
                };

                if (cardRequestContext.RequestCard is Sha && PlayerContext.Player.SkillButtonInfoList.Any(s => s.IsEnabled && s is IAbility sa && sa.CanProvideSha()))
                {
                    return await showMsg();
                }
                else if (cardRequestContext.RequestCard is Wuxiekeji && PlayerContext.Player.SkillButtonInfoList.Any(s => s.IsEnabled && s is IAbility sa && sa.CanProviderWuxiekeji()))
                {
                    return await showMsg();
                }
                else if (cardRequestContext.RequestCard is Yao && PlayerContext.Player.SkillButtonInfoList.Any(s => s.IsEnabled && s is IAbility sa && sa.CanProvideYao()))
                {
                    return await showMsg();
                }
            }
            //默认返回失败
            return new CardResponseContext() { };
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
