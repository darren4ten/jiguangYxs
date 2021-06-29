using Logic.Enums;
using Logic.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logic.ActionManger;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;
using Logic.Model.Cards.Interface;
using Logic.Model.Cards.MutedCards;
using Logic.Model.Player;
using Logic.Model.RequestResponse.Request;
using Logic.Model.RequestResponse.Response;
using Logic.Log;

namespace Logic.Cards
{
    public abstract class CardBase : INotifyPropertyChanged
    {
        #region 属性

        private int _cardId;
        public int CardId
        {
            get { return _cardId; }
            set
            {
                _cardId = value;
                OnPropertyChanged();
            }
        }

        private string _name { get; set; }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否对别人可见
        /// </summary>
        private bool _isViewableForOthers = true;
        public bool IsViewableForOthers
        {
            get { return _isViewableForOthers; }
            set
            {
                _isViewableForOthers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }

        private string _displayNumberText;
        public string DisplayNumberText
        {
            get { return _displayNumberText; }
            set
            {
                _displayNumberText = value;
                OnPropertyChanged();
            }
        }

        private bool _isPopout;
        /// <summary>
        /// 是否是弹出状态
        /// </summary>
        public bool IsPopout
        {
            get { return _isPopout; }
            set
            {
                _isPopout = value;
                OnPropertyChanged();
            }
        }

        private int _num;
        /// <summary>
        /// 卡牌数字
        /// </summary>
        public int Number
        {
            get { return _num; }
            set
            {
                _num = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Number"));
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string FlowerKindString => GetFlowerKind();
        /// <summary>
        /// 花色
        /// </summary>
        private FlowerKindEnum _flowerKind;
        public FlowerKindEnum FlowerKind
        {
            get { return _flowerKind; }
            set
            {
                _flowerKind = value;
                OnPropertyChanged();
            }
        }

        private CardColorEnum? _Color = null;

        /// <summary>
        /// 颜色
        /// </summary>
        public CardColorEnum Color
        {
            get
            {
                return _Color == null ? (FlowerKind == FlowerKindEnum.Hongtao || FlowerKind == FlowerKindEnum.Fangkuai ? CardColorEnum.Red : CardColorEnum.Black) : _Color.Value;
            }
            set
            {
                _Color = value;
                OnPropertyChanged();
            }
        }

        private const string CardBackImage = "\\Resources\\card\\card_back.jpg";
        /// <summary>
        /// 图像path
        /// </summary>
        private string _img;
        public string Image
        {
            get { return IsViewableForOthers ? _img : CardBackImage; }
            set
            {
                _img = value;
                OnPropertyChanged();
            }
        }

        public PlayerContext PlayerContext { get; private set; }

        #endregion

        /// <summary>
        /// 是否能够被主动打出
        /// </summary>
        public abstract bool CanBePlayed();

        /// <summary>
        /// 选择目标,主要用来当前用户需要UI操作选择目标
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<SelectedTargetsResponse> SelectTargets(SelectedTargetsRequest request)
        {
            var cardRequest = CardRequestContext.GetBaseCardRequestContext(null);
            cardRequest.AdditionalContext = request;
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeSelectTarget,
                PlayerContext.Player.CurrentPlayerHero, request.CardRequest,
                PlayerContext.Player.RoundContext, new CardResponseContext());
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.SelectTarget,
                PlayerContext.Player.CurrentPlayerHero, request.CardRequest,
                PlayerContext.Player.RoundContext, new CardResponseContext());

            var res = await PlayerContext.Player.ActionManager.OnRequestSelectTargets(request);

            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.AfterSelectTarget,
                PlayerContext.Player.CurrentPlayerHero, request.CardRequest,
                PlayerContext.Player.RoundContext, new CardResponseContext());
            return res;
        }

        /// <summary>
        /// 绑定卡牌的上下文
        /// </summary>
        /// <param name="playerContext"></param>
        /// <returns></returns>
        public CardBase AttachPlayerContext(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
            return this;
        }

        ///// <summary>
        ///// 目标数量
        ///// </summary>
        ///// <returns></returns>
        //public virtual SelectedTargetsRequest GetSelectTargetRequest()
        //{
        //    return new SelectedTargetsRequest()
        //    {
        //        MinTargetCount = 1,
        //        MaxTargetCount = 1,
        //        CardRequest = CardRequestContext.GetBaseCardRequestContext(),
        //        RoundContext = PlayerContext.Player.RoundContext,
        //        TargetType = AttackTypeEnum.Sha
        //    };
        //}

        /// <summary>
        /// 能够被动出牌
        /// </summary>
        /// <returns></returns>
        public static bool CanBeidongPlayCard<T>(PlayerContext playerContext)
        {
            if (playerContext.Player.IsInBeidongMode() &&
                (playerContext.Player.CurrentCardRequestContext.RequestCard == null || playerContext.Player.CurrentCardRequestContext.RequestCard is T))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断给定的卡是否是指定的牌
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool Is<T>(CardBase card) where T : CardBase
        {
            if (card is ChangedCard c)
            {
                return c.TargetCard is T;
            }

            return card is T;
        }

        #region 主动出牌，Play card
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        public virtual async Task<CardResponseContext> PlayCard(CardRequestContext cardRequestContext, RoundContext roundContext)
        {
            cardRequestContext.AttackDynamicFactor = cardRequestContext.AttackDynamicFactor ??
                                                     AttackDynamicFactor.GetDefaultBaseAttackFactor();
            //默认SrcPlayer为当前出牌的人
            cardRequestContext.SrcPlayer = cardRequestContext.SrcPlayer ?? PlayerContext.Player;
            Console.WriteLine($"[{cardRequestContext.SrcPlayer.PlayerName}{cardRequestContext.SrcPlayer.PlayerId}]的【{cardRequestContext.SrcPlayer.CurrentPlayerHero.Hero.DisplayName}】{(cardRequestContext.TargetPlayers?.Any() == true ? "向" + string.Join(",", cardRequestContext.TargetPlayers.Select(p => p.PlayerName + p.PlayerId)) : "")}打出“{this.ToString()}”");
            PlayerContext.GameLevel.LogManager.LogAction(
                new RichTextParagraph(
                 new RichTextWrapper($"{cardRequestContext.SrcPlayer.PlayerId}【{cardRequestContext.SrcPlayer.CurrentPlayerHero.Hero.DisplayName}】", RichTextWrapper.GetColor(ColorEnum.Blue), 12, true),
                 new RichTextWrapper((cardRequestContext.TargetPlayers?.Any() == true ? "向" + string.Join(",", cardRequestContext.TargetPlayers.Select(p => p)) : "")),
                 new RichTextWrapper("打出"),
                 new RichTextWrapper("“"),
                 new RichTextWrapper(ToString(), RichTextWrapper.GetColor(ColorEnum.Red)),
                 new RichTextWrapper("”")
                 ));

            CardResponseContext responseContext = new CardResponseContext();
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforeZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            if (responseContext.ResponseResult != ResponseResultEnum.UnKnown) return responseContext;

            var r1 = await OnBeforePlayCard(cardRequestContext, responseContext, roundContext);
            if (r1.ResponseResult != ResponseResultEnum.UnKnown) return r1;

            //将该牌置入TempCardDesk
            await PlayerContext.Player.RemoveCardsInHand(new List<CardBase>() { this }, cardRequestContext, responseContext,
                 roundContext);

            await PlayerContext.Player.TriggerEvent(EventTypeEnum.ZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            var r2 = await OnPlayCard(cardRequestContext, r1, roundContext);

            var r3 = await OnAfterPlayCard(cardRequestContext, r2, roundContext);
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.AfterZhudongPlayCard, cardRequestContext, responseContext, roundContext);


            return r3;
        }

        /// <summary>
        /// 出牌之前
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected virtual async Task<CardResponseContext> OnBeforePlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            return await Task.FromResult(new CardResponseContext());
        }

        /// <summary>
        /// 出牌中
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected virtual async Task<CardResponseContext> OnPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            return await Task.FromResult(new CardResponseContext());
        }

        /// <summary>
        /// 出牌后
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected virtual async Task<CardResponseContext> OnAfterPlayCard(CardRequestContext cardRequestContext, CardResponseContext cardResponseContext, RoundContext roundContext)
        {
            return await Task.FromResult(new CardResponseContext());
        }

        #endregion

        #region 被动出牌，Response card
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        public virtual async Task<CardResponseContext> ResponseCard(CardRequestContext cardRequestContext, RoundContext roundContext)
        {
            var response = await OnBeforeResponseCard(cardRequestContext, roundContext);
            var response1 = await OnResponseCard(cardRequestContext, roundContext, response);
            return await OnAfterResponseCardCard(cardRequestContext, roundContext, response1);
        }

        /// <summary>
        /// 出牌之前
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected virtual async Task<CardResponseContext> OnBeforeResponseCard(CardRequestContext cardRequestContext, RoundContext roundContext)
        {
            return await Task.FromResult<CardResponseContext>(null);
        }

        /// <summary>
        /// 出牌中
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected virtual async Task<CardResponseContext> OnResponseCard(CardRequestContext cardRequestContext, RoundContext roundContext, CardResponseContext responseContext)
        {
            return await Task.FromResult<CardResponseContext>(null);
        }

        /// <summary>
        /// 出牌后
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected virtual async Task<CardResponseContext> OnAfterResponseCardCard(CardRequestContext cardRequestContext, RoundContext roundContext, CardResponseContext responseContext)
        {
            return await Task.FromResult<CardResponseContext>(null);
        }


        #endregion

        protected async Task<CardResponseContext> CheckResponse(CardRequestContext cardRequestContext, CardResponseContext actResponse, RoundContext roundContext)
        {
            if (actResponse == null)
            {
                return await Task.FromResult(new CardResponseContext()
                {
                    ResponseResult = ResponseResultEnum.Failed
                });
            }

            if (actResponse.ResponseResult != ResponseResultEnum.UnKnown)
            {
                return actResponse;
            }

            if (actResponse.Cards != null && actResponse.Cards.Count >= cardRequestContext.MinCardCountToPlay &&
                actResponse.Cards.Count <= cardRequestContext.MaxCardCountToPlay)
            {
                actResponse.ResponseResult = ResponseResultEnum.Success;
                return actResponse;
            }
            else
            {
                actResponse.ResponseResult = ResponseResultEnum.Failed;
                return actResponse;
            }
        }


        /// <summary>
        /// 主动打牌，弹出卡牌。即如果该牌可以打出，则打出
        /// </summary>
        /// <returns>返回是否出过牌</returns>
        public virtual async Task<bool> Popup(CardRequestContext request = null)
        {
            if (CanBePlayed())
            {
                request = request ?? CardRequestContext.GetBaseCardRequestContext(null);
                if (this is INeedTargets)
                {
                    var selectRequest = ((INeedTargets)this).GetSelectTargetRequest();
                    selectRequest.SelectTargetsRequestTaskCompletionSource = new TaskCompletionSource<SelectedTargetsResponse>();
                    var targetResponse = await SelectTargets(selectRequest);
                    if (selectRequest.MinTargetCount == 0
                        || (targetResponse?.Targets != null && targetResponse.Targets.Count >= selectRequest.MinTargetCount))
                    {
                        request.SrcPlayer = PlayerContext.Player;
                        request.TargetPlayers = targetResponse.Targets;
                        //request.AttackType = selectRequest.TargetType;
                        return await ShowPlayButton(async () =>
                        {
                            await PlayCard(request, PlayerContext.Player.RoundContext);
                        });
                    }
                }
                else
                {
                    //如果是人类，则展示确认按钮
                    return await ShowPlayButton(async () =>
                    {
                        request.SrcPlayer = PlayerContext.Player;
                        await PlayCard(request, PlayerContext.Player.RoundContext);
                    });

                }
            }

            return false;
        }

        private async Task<bool> ShowPlayButton(Func<Task> action)
        {
            if (!PlayerContext.IsAi())
            {
                TaskCompletionSource<CardResponseContext> tcs = new TaskCompletionSource<CardResponseContext>();

                string displayMessage = "";
                PlayerContext.Player.PlayerUiState.SetupOkCancelActionBar(tcs, displayMessage, "确定", "");
                var res = await tcs.Task;
                await action();
                if (res.ResponseResult == ResponseResultEnum.Success)
                {
                    return true;
                }
                else
                {
                    //如果没有确认，则不出牌
                    return false;
                }
            }
            await action();
            //如果是机器人，则直接出牌
            return true;
        }

        private string GetFlowerKind()
        {
            var flower = "";
            switch (FlowerKind)
            {
                case FlowerKindEnum.Fangkuai:
                    {
                        flower = "♦";
                    };
                    break;
                case FlowerKindEnum.Meihua:
                    {
                        flower = "♣";
                    };
                    break;
                case FlowerKindEnum.Heitao:
                    {
                        flower = "♠";
                    };
                    break;
                case FlowerKindEnum.Hongtao:
                    {
                        flower = "♥";
                    };
                    break;
                default:
                    {
                        flower = "♣";
                    };
                    break;
            }
            return flower;
        }

        public override string ToString()
        {
            var flower = GetFlowerKind();
            return $"[{flower}{Number} {DisplayName}]";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
