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

        #region 主动出牌，Play card
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        public virtual async Task PlayCard(CardRequestContext cardRequestContext, RoundContext roundContext)
        {
            await OnBeforePlayCard(cardRequestContext, roundContext);
            await OnPlayCard(cardRequestContext, roundContext);
            await OnAfterPlayCard(cardRequestContext, roundContext);
        }

        /// <summary>
        /// 出牌之前
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected virtual async Task OnBeforePlayCard(CardRequestContext cardRequestContext, RoundContext roundContext)
        {
            await Task.FromResult("");
        }

        /// <summary>
        /// 出牌中
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected virtual async Task OnPlayCard(CardRequestContext cardRequestContext, RoundContext roundContext)
        {
            await Task.FromResult("");
        }

        /// <summary>
        /// 出牌后
        /// </summary>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <returns></returns>
        protected virtual async Task OnAfterPlayCard(CardRequestContext cardRequestContext, RoundContext roundContext)
        {
            await Task.FromResult("");
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
