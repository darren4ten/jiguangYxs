using System;
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
        public StandardActionManager() { }
        public StandardActionManager(PlayerContext playContext) : base(playContext)
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
            return await OnRequestResponseCard(cardRequestContext);
        }

        public override async Task<CardResponseContext> OnRequestResponseCard(CardRequestContext cardRequestContext)
        {
            string displayMessage = $"是否打出“{cardRequestContext.RequestCard.DisplayName}?”";
            //todo:如果武器牌可选，则高亮武器牌

            //处理选择牌的请求
            if (cardRequestContext.AttackType == AttackTypeEnum.SelectCard)
            {
                return await OnRequestPickCardFromPanel(new PickCardFromPanelRequest()
                {
                    MaxCount = cardRequestContext.MaxCardCountToPlay,
                    MinCount = cardRequestContext.MinCardCountToPlay,
                    Panel = cardRequestContext.Panel,
                    RequestId = cardRequestContext.RequestId
                });
            }

            //监听卡牌的弹出事件，如果卡牌弹出，则检查是否符合出牌要求：
            //1. 花色要求
            //2. 具体卡牌要求
            //3. 出牌数量要求
            PlayerContext.Player.PlayerUiState.OnCardInHandClicked = async (sender) =>
            {
                if (sender is CardBase card)
                {
                    card.IsPopout = !card.IsPopout;

                    //如果是弹出状态，检查要求。如果能够满足要求，则弹出确认、取消按钮
                    //满足的要求：出牌数小于等于最大出牌数，出牌满足花色和出牌要求
                    //典型场景：
                    //1. 出一张杀或者闪
                    //2. 出一张红桃
                    //3. 出两张手牌
                    //4. 出两张杀或两张闪

                    //检查手牌所有IsPopout的牌
                    if (CanShowPlayButton(cardRequestContext))
                    {
                        PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(cardRequestContext.RequestTaskCompletionSource, displayMessage, "确定", "取消");
                    }
                }
                return await Task.FromResult(false);
            };
            PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(cardRequestContext.RequestTaskCompletionSource, displayMessage, null, "取消");
            var res = await cardRequestContext.RequestTaskCompletionSource.Task;
            return res;
        }

        public override async Task<CardResponseContext> OnRequestPickCardFromPanel(PickCardFromPanelRequest request)
        {
            request.RequestTaskCompletionSource =
                request.RequestTaskCompletionSource ?? new TaskCompletionSource<CardResponseContext>();
            //弹出窗体，提示选择牌，
            PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(request.RequestTaskCompletionSource, request.Panel.DisplayMessage, null, null);
            PlayerContext.Player.PlayerUiState.Panel.OnClickedHandler = async (sender) =>
                {
                    if (sender is PanelCard card)
                    {
                        if (CanSelectPanelCard(card))
                        {
                            request.RequestTaskCompletionSource.SetResult(new CardResponseContext()
                            {
                                Cards = new List<CardBase>() { card.Card }
                            });
                        }
                        else
                        {

                        }
                    }
                    return await Task.FromResult(false);
                };
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
            //请求出牌时，提示请求出牌
            var tcs = PlayerContext.Player.RoundContext.RoundTaskCompletionSource ?? new TaskCompletionSource<CardResponseContext>();
            PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(tcs, "请出牌", null, "取消");
            await tcs.Task;
        }

        public override async Task<List<CardBase>> OnRequestStartStep_ThrowCard(int throwCount)
        {
            var tcs = PlayerContext.Player.RoundContext.RoundTaskCompletionSource ?? new TaskCompletionSource<CardResponseContext>();
            PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(tcs, $"请弃掉{throwCount}张牌", null, null);
            //设置手牌的点击事件为选择牌
            PlayerContext.Player.PlayerUiState.OnCardInHandClicked = async (sender) =>
            {
                if (sender is CardBase card)
                {
                    card.IsPopout = !card.IsPopout;
                    //检查手牌所有IsPopout的牌
                    if (CanShowThrowButton(throwCount))
                    {
                        PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(tcs, "", "确定", null, (async () =>
                            {
                                tcs.SetResult(new CardResponseContext()
                                {
                                    ResponseResult = ResponseResultEnum.Success,
                                    Cards = PlayerContext.Player.CardsInHand.Where(p => p.IsPopout).ToList()
                                });
                                return await Task.FromResult(true);
                            }));
                    }
                    else
                    {
                        PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(tcs, $"请选择{throwCount}张牌", null, null);
                    }
                }
                return await Task.FromResult(false);
            };
            var res = await tcs.Task;
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

            if (response.ResponseResult == ResponseResultEnum.Success)
            {
                return new SelectedTargetsResponse()
                {
                    Status = ResponseResultEnum.Success,
                    Targets = PlayerContext.Player.PlayerUiState.GetSelectedTargets()
                };
            }

            //  1.1 如果满足了要求，则弹窗显示确定、取消按钮，如果确定，则返回selecte目标，如果取消，则返回失败
            return new SelectedTargetsResponse()
            {
                Status = ResponseResultEnum.Failed,
            };
        }

        #region 私有逻辑
        /// <summary>
        /// 面板牌能否被选中
        /// </summary>
        /// <param name="pCard"></param>
        /// <returns></returns>
        private bool CanSelectPanelCard(PanelCard pCard)
        {
            return pCard != null && pCard.SelectedBy == null;
        }

        /// <summary>
        /// 是否显示主动出牌的“确认”按钮
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <returns></returns>
        private bool CanShowPlayButton(CardRequestContext cardRequestContext)
        {
            //检查手牌所有IsPopout的牌
            var pendingPlayCards = PlayerContext.Player.CardsInHand.Where(p => p.IsPopout).ToList();
            if (
                 pendingPlayCards.Count() <= cardRequestContext.MaxCardCountToPlay && pendingPlayCards.Count() >= cardRequestContext.MinCardCountToPlay
            )
            {
                foreach (var p in pendingPlayCards)
                {
                    if (cardRequestContext.RequestCard.GetType() != p.GetType())
                    {
                        return false;
                    }
                    if (cardRequestContext.FlowerKind != Enums.FlowerKindEnum.Any)
                    {
                        if (cardRequestContext.FlowerKind != p.FlowerKind)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否显示弃牌按钮
        /// </summary>
        /// <param name="throwCount"></param>
        /// <returns></returns>
        private bool CanShowThrowButton(int throwCount)
        {
            var popedCount = PlayerContext.Player.CardsInHand.Count(p => p.IsPopout);
            return popedCount == throwCount;
        }
        #endregion
    }
}
