using Logic.Enums;
using Logic.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;

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

        /// <summary>
        /// 卡牌类型
        /// </summary>
        public CardTypeEnum CardType { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public CardColorEnum Color { get; set; }

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

        public CardBase AttachPlayerContext(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
            return this;
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
            CardResponseContext responseContext = new CardResponseContext();
            await PlayerContext.GameLevel.GlobalEventBus.TriggerEvent(EventTypeEnum.BeforeSha, cardRequestContext, roundContext, responseContext);
            if (responseContext.ResponseResult != ResponseResultEnum.UnKnown) return responseContext;

            var r1 = await OnBeforePlayCard(cardRequestContext, responseContext, roundContext);
            if (r1.ResponseResult != ResponseResultEnum.UnKnown) return r1;

            var r2 = await OnPlayCard(cardRequestContext, r1, roundContext);
            if (r2.ResponseResult != ResponseResultEnum.UnKnown) return r2;

            var r3 = await OnAfterPlayCard(cardRequestContext, r2, roundContext);
            if (r3.ResponseResult != ResponseResultEnum.UnKnown) return r3;
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

        public abstract Task Popup();


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
