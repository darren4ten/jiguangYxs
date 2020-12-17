using Logic.Enums;
using Logic.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Model.Cards.BaseCards;

namespace Logic.Cards
{
    public abstract class CardBase : INotifyPropertyChanged
    {
        public int CardId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 是否对别人可见
        /// </summary>
        public bool IsViewableForOthers { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        public string DisplayNumberText { get; set; }

        /// <summary>
        /// 卡牌数字
        /// </summary>
        public int Number { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 花色
        /// </summary>
        public FlowerKindEnum FlowerKind { get; set; }

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
            set => _Color = value;
        }

        /// <summary>
        /// 图像path
        /// </summary>
        public string Image { get; set; }

        public PlayerContext PlayerContext { get; private set; }
        /// <summary>
        /// 是否能够被主动打出
        /// </summary>
        public abstract bool CanBePlayed();

        /// <summary>
        /// 选择目标,主要用来当前用户需要UI操作选择目标
        /// </summary>
        /// <returns></returns>
        public virtual async Task SelectTargets()
        {
            await Task.FromResult("");
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

        /// <summary>
        /// 能够被动出牌
        /// </summary>
        /// <returns></returns>
        public static bool CanBeidongPlayCard<T>(PlayerContext playerContext)
        {
            if (playerContext.Player.IsInBeidongMode() &&
                (playerContext.Player.CardRequestContext.RequestCard == null || playerContext.Player.CardRequestContext.RequestCard is T))
            {
                return true;
            }
            return false;
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
            //默认SrcPlayer为当前出牌的人
            cardRequestContext.SrcPlayer = cardRequestContext.SrcPlayer ?? PlayerContext.Player;
            Console.WriteLine($"[{cardRequestContext.SrcPlayer.PlayerName}{cardRequestContext.SrcPlayer.PlayerId}]的【{cardRequestContext.SrcPlayer.GetCurrentPlayerHero().Hero.DisplayName}】{(cardRequestContext.TargetPlayers?.Any() == true ? "向" + string.Join(",", cardRequestContext.TargetPlayers.Select(p => p.PlayerName + p.PlayerId)) : "")}打出“{this.ToString()}”");

            CardResponseContext responseContext = new CardResponseContext();
            await PlayerContext.Player.TriggerEvent(EventTypeEnum.BeforeZhudongPlayCard, cardRequestContext, responseContext, roundContext);
            if (responseContext.ResponseResult != ResponseResultEnum.UnKnown) return responseContext;

            var r1 = await OnBeforePlayCard(cardRequestContext, responseContext, roundContext);
            if (r1.ResponseResult != ResponseResultEnum.UnKnown) return r1;

            //将该牌置入TempCardDesk
            PlayerContext.Player.CardsInHand.Remove(this);
            PlayerContext.GameLevel.TempCardDesk.Add(this);

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

        public override string ToString()
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
            return $"[{flower}{Number} {DisplayName}]";
        }

        public abstract Task Popup();


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
