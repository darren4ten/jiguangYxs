using Logic.Cards;
using Logic.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logic.Event;
using Logic.Model.Cards.MutedCards;
using Logic.Model.Enums;
using Logic.Model.Player;

namespace Logic.GameLevel
{
    public abstract class GameLevelBase
    {
        /// <summary>
        /// 当前关卡全局事件管理器
        /// </summary>
        public readonly EventBus GlobalEventBus = new EventBus();

        public int LevelId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        protected Queue<CardBase> UnUsedCardStack { get; set; }
        protected Queue<CardBase> UsedCardStack { get; set; }

        protected List<Player> Players { get; set; }

        /// <summary>
        /// todo:用来存放回合所出的牌,回合结束的时候放置入弃牌堆
        /// </summary>
        public TempCardDesk TempCardDesk { get; set; }

        protected List<CardBase> CardsOnDesk { get; set; }

        /// <summary>
        /// 重新洗牌
        /// </summary>
        protected virtual void WashCards()
        {
            var unUsedCardList = new List<CardBase>();
            var util = new CardStackUtil();
            //todo:触发洗牌动画
            if (UnUsedCardStack != null && UnUsedCardStack.Any())
            {
                unUsedCardList.AddRange(UnUsedCardStack);
                unUsedCardList.AddRange(util.GenerateNewCardStack(UsedCardStack.ToList()));
            }
            else
            {
                unUsedCardList = util.GenerateNewCardStack().ToList();
            }
            this.UnUsedCardStack = new Queue<CardBase>(unUsedCardList);
            this.UsedCardStack = new Queue<CardBase>();
        }

        protected virtual void LoadPlayers()
        {
            this.Players = new List<Player>();
        }

        /// <summary>
        /// 摸牌
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual IEnumerable<CardBase> PickNextCardsFromStack(int count)
        {
            //如果取牌数>=当前剩余牌数，则重新洗牌之后再取牌
            if (count >= UnUsedCardStack.Count)
            {
                WashCards();
            }
            for (int i = 0; i < count; i++)
            {
                yield return UnUsedCardStack.Dequeue();
            }
        }

        public virtual async Task<CardResponseContext> Panding(CardRequestContext request, Expression<Func<CardBase, bool>> expression)
        {
            var response = new CardResponseContext() { Cards = new List<CardBase>() };
            await GlobalEventBus.TriggerEvent(EventTypeEnum.BeforePanding, null, request, null, response);
            await GlobalEventBus.TriggerEvent(EventTypeEnum.Panding, null, request, null, response);
            if (response.ResponseResult == ResponseResultEnum.Cancelled)
            {
                return response;
            }
            var pdCard = PickNextCardsFromStack(1).FirstOrDefault();
            response.Cards.Add(pdCard);
            var isSuccess = expression.Compile().Invoke(pdCard);
            if (isSuccess)
            {
                response.ResponseResult = ResponseResultEnum.Success;
            }
            await GlobalEventBus.TriggerEvent(EventTypeEnum.AfterPanding, null, request, null, response);
            return response;
        }

        /// <summary>
        /// 将牌置于弃牌堆
        /// </summary>
        /// <param name="cards"></param>
        public virtual void ThrowCardToStack(List<CardBase> cards)
        {
            foreach (var card in cards)
            {
                if (card is CombinedCard)
                {
                    ThrowBaseCardsToStack((card as CombinedCard).OriginalCards);
                }
                else if (card is ChangedCard)
                {
                    ThrowBaseCardsToStack((card as ChangedCard).OriginalCards);
                }
                else
                {
                    ThrowBaseCardsToStack(new List<CardBase>() { card });
                }
            }
        }

        public virtual void OnLoad()
        {
            this.TempCardDesk = new TempCardDesk();
            this.WashCards();
            this.LoadPlayers();
        }

        public virtual void Start()
        {
            Console.WriteLine("Game Started!");
        }

        public virtual void End()
        {
            //释放所有对象,players,skills,eventbus

            this.Players = null;
            this.UnUsedCardStack = null;
            this.UsedCardStack = null;
            //Play animation
            Console.WriteLine("Game over!");
        }

        private void ThrowBaseCardsToStack(List<CardBase> cards)
        {
            foreach (var cardBase in cards)
            {
                UnUsedCardStack.Enqueue(cardBase);
            }
        }
    }
}
