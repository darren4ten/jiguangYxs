﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Cards;
using Logic.GameLevel;
using Logic.GameLevel.Panel;
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
        public StandardActionManager()
        {

        }

        public StandardActionManager(PlayerContext playContext) : base(playContext)
        {
            PlayerContext.Player.PlayerUiState.ActionBar = new ActionBar();
        }

        public override void Setup()
        {
            PlayerContext.Player.PlayerUiState.ActionBar = new ActionBar();
        }

        /// <summary>
        /// 请求触发技能
        /// </summary>
        /// <param name="skillType"></param>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        public override async Task<bool> OnRequestTriggerSkill(SkillTypeEnum skillType, CardRequestContext cardRequestContext)
        {
            //询问是否发动技能，一般只有被动技能才会需要询问，比如：集权、反击、鲁莽
            //提示技能消息
            //弹出确认、取消按钮
            //如果点击确认或者取消则返回true或者false

            string displayMessage = $"是否发动“{skillType.GetDescription()}?”";
            var tcs = new TaskCompletionSource<CardResponseContext>();
            PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(tcs, displayMessage, "确认", "取消");
            var res = await tcs.Task;
            return res != null && res.ResponseResult == ResponseResultEnum.Success ? true : false;
        }

        /// <summary>
        /// 并发请求出牌，只要有一个人出过牌就结束
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        public override async Task<CardResponseContext> OnParallelRequestResponseCard(CardRequestContext cardRequestContext)
        {
            return await PlayerContext.Player.ResponseCard(cardRequestContext, null, null);
            //return await OnRequestResponseCard(cardRequestContext);
        }

        public override async Task<CardResponseContext> OnRequestResponseCard(CardRequestContext cardRequestContext)
        {
            string displayMessage = string.IsNullOrEmpty(cardRequestContext?.Message) ? GetMessage(cardRequestContext) : cardRequestContext.Message;
            //todo:如果武器牌可选，则高亮武器牌
            var tcs = cardRequestContext.RequestTaskCompletionSource ?? new TaskCompletionSource<CardResponseContext>();
            //处理选择牌的请求
            if (cardRequestContext.AttackType == AttackTypeEnum.SelectCard)
            {
                return await OnRequestPickCardFromPanel(new PickCardFromPanelRequest()
                {
                    MaxCount = cardRequestContext.MaxCardCountToPlay,
                    MinCount = cardRequestContext.MinCardCountToPlay,
                    Panel = cardRequestContext.Panel,
                    RequestId = cardRequestContext.RequestId,
                    RequestTaskCompletionSource = tcs
                });
            }

            PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(tcs, displayMessage, null, "取消", btn2EventHandler: (
                async () =>
                {
                    tcs.SetResult(new CardResponseContext() { ResponseResult = ResponseResultEnum.Failed });
                    return await Task.FromResult(false);
                }));
            var res = await tcs.Task;
            return res;
        }

        public override async Task<CardResponseContext> OnRequestPickCardFromPanel(PickCardFromPanelRequest request)
        {
            request.RequestTaskCompletionSource =
                request.RequestTaskCompletionSource ?? new TaskCompletionSource<CardResponseContext>();
            request.Panel.OnClickedHandler = async (card, uiAction) =>
            {
                card.IsPopout = true;
                //检查选中的卡牌数量是不是match
                var pCards = request.Panel.GetSelectedCards().ToList();
                pCards.ForEach(p => p.SelectedBy = this.PlayerContext.Player);
                var cards = pCards.Select(c => c.Card).ToList();
                if (request.IsMatch(cards))
                {
                    request.RequestTaskCompletionSource.SetResult(new CardResponseContext()
                    {
                        ResponseResult = ResponseResultEnum.Success,
                        Cards = cards
                    });
                }

                await Task.FromResult(0);
            };
            //弹出窗体，提示选择牌，
            await PlayerContext.Player.PlayerUiState.ShowPanel(request.Panel);
            var r = await request.RequestTaskCompletionSource.Task;

            return r;
        }

        public override async Task OnRequestStartStep_EnterMyRound()
        {
            await Task.FromResult(0);
        }

        public override async Task OnRequestStartStep_PickCard()
        {
            await Task.FromResult(0);
        }

        public override async Task OnRequestStartStep_PlayCard()
        {
            PlayerContext.Player.RoundContext.RoundTaskCompletionSource =
                new TaskCompletionSource<CardResponseContext>();
            //请求出牌时，提示请求出牌
            PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(PlayerContext.Player.RoundContext.RoundTaskCompletionSource, "请出牌", null, "结束出牌");
            await PlayerContext.Player.RoundContext.RoundTaskCompletionSource.Task;
        }

        public override async Task<List<CardBase>> OnRequestStartStep_ThrowCard(int throwCount)
        {
            if (throwCount <= 0)
            {
                return null;
            }

            var newCardRequestContext = new CardRequestContext()
            {
                RequestTaskCompletionSource = new TaskCompletionSource<CardResponseContext>(),
                MinCardCountToPlay = throwCount,
                MaxCardCountToPlay = throwCount,
                AttackType = AttackTypeEnum.ThrowCard
            };
            PlayerContext.Player.CardRequestContexts.Add(newCardRequestContext);

            PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(newCardRequestContext.RequestTaskCompletionSource, $"请弃掉{throwCount}张牌", "确定", null, (async () =>
            {
                newCardRequestContext.RequestTaskCompletionSource.SetResult(new CardResponseContext()
                {
                    ResponseResult = ResponseResultEnum.Success,
                    Cards = PlayerContext.Player.CardsInHand.Where(p => p.IsPopout).ToList()
                });
                //确定出牌后清理掉这个request，否则下一次主动出牌时会变成弃牌
                await PlayerContext.Player.RemoveCardRequestContext(newCardRequestContext.RequestId);
                return await Task.FromResult(true);
            }));
            //默认弃牌的时候不能出牌
            PlayerContext.Player.PlayerUiState.CardsInHandHandler =
                PlayerContext.Player.PlayerUiState.ThrowCardCardInHandClicked;
            PlayerContext.Player.PlayerUiState.ActionBar.BtnAction1.IsEnabled = false;
            var res = await newCardRequestContext.RequestTaskCompletionSource.Task;
            return res.Cards;
        }

        public override async Task OnRequestStartStep_ExitMyRound()
        {
            await Task.FromResult(0);
        }

        /// <summary>
        /// 请求选择目标
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override async Task<SelectedTargetsResponse> OnRequestSelectTargets(SelectedTargetsRequest request)
        {
            if (request.MinTargetCount <= 0 && request.MaxTargetCount <= 0)
            {
                return new SelectedTargetsResponse()
                {
                    Status = ResponseResultEnum.UnKnown
                };
            }
            string displayMessage = $"请选择{request.MinTargetCount}{(request.MaxTargetCount > request.MinTargetCount ? "-" + request.MaxTargetCount : "")}个目标";
            //弹窗提示选择目标
            PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(null, displayMessage, null, null);
            //UI更新可供选择的目标，将可选的目标高亮出来，
            //1. 监听玩家被click的事件，如果click了，则检查目标是否可选且目标数目是不是达到选择目标的要求，
            var response = await PlayerContext.Player.PlayerUiState.HighlightAvailableTargets(request, null);
            //2. 完成目标选择之后（取消或者确定），恢复玩家的选中状态为idle.
            PlayerContext.Player.PlayerUiState.RestoreSelectStatus(SelectStatusEnum.Idle);
            return response;
        }

        #region 私有逻辑

        private string GetMessage(CardRequestContext cardRequestContext)
        {
            if (cardRequestContext == null)
            {
                return null;
            }
            string message = "是否打出";

            if (cardRequestContext.MaxCardCountToPlay > cardRequestContext.MinCardCountToPlay)
            {
                message += cardRequestContext.MinCardCountToPlay + "-" + cardRequestContext.MaxCardCountToPlay + "张";
            }
            else if (cardRequestContext.MaxCardCountToPlay == cardRequestContext.MinCardCountToPlay && cardRequestContext.MaxCardCountToPlay > 1)
            {
                message += cardRequestContext.MinCardCountToPlay + "张";
            }

            message += "“" + cardRequestContext.RequestCard?.DisplayName + "”";
            return message;
        }

        /// <summary>
        /// 面板牌能否被选中
        /// </summary>
        /// <param name="pCard"></param>
        /// <returns></returns>
        private bool CanSelectPanelCard(PanelCard pCard)
        {
            return pCard != null && pCard.SelectedBy == null;
        }

        #endregion
    }
}
