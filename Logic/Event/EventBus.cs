﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.GameLevel;
using Logic.Model.Enums;
using Logic.Model.Player;

namespace Logic.Event
{
    public class EventBus
    {
        public delegate Task RoundEventHandler(CardRequestContext cardRequestContext, RoundContext roundContext,
            CardResponseContext cardResponseContext);

        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="evtId">事件Id</param>
        /// <param name="player"></param>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public void ListenEvent(Guid evtId, PlayerHero player, EventTypeEnum eventType, RoundEventHandler handler)
        {
            var eventDic = EventDic.ContainsKey(eventType) ? EventDic[eventType] : null;
            if (eventDic != null)
            {
                if (eventDic.ContainsKey(player))
                {
                    eventDic[player] = eventDic[player] ?? new EventData() { EventHandlers = new List<EventHandler>() };
                    eventDic[player].EventHandlers = eventDic[player].EventHandlers ?? new List<EventHandler>();
                    eventDic[player].EventHandlers.Add(new EventHandler()
                    {
                        EventId = evtId,
                        RoundEventHandler = handler
                    });
                }
                else
                {
                    eventDic.TryAdd(player, new EventData()
                    {
                        EventHandlers = new List<EventHandler>()
                        {
                            new EventHandler()
                            {
                                EventId = evtId,
                                RoundEventHandler = handler
                            }
                        }
                    });
                }
            }
            else
            {
                var dic = new ConcurrentDictionary<PlayerHero, EventData>();
                dic.TryAdd(player, new EventData()
                {
                    EventHandlers = new List<EventHandler>()
                    {
                        new EventHandler()
                        {
                            EventId = evtId,
                            RoundEventHandler = handler
                        }
                    }
                });
                EventDic.TryAdd(eventType, dic);
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="evtId">要移除的事件Id，可为空，如果为空，则代表将整个对应的EventType对应的事件全部移除掉</param>
        public void RemoveEventListener(EventTypeEnum eventType, Guid? evtId)
        {
            if (EventDic.ContainsKey(eventType))
            {
                var eventBinds = EventDic[eventType];
                if (evtId == null)
                {
                    EventDic.TryRemove(eventType, out eventBinds);
                    return;
                }
                foreach (var evtBind in eventBinds)
                {
                    evtBind.Value.EventHandlers?.RemoveAll(p => p.EventId == evtId);
                }
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="cardRequestContext"></param>
        /// <param name="roundContext"></param>
        /// <param name="cardResponseContext"></param>
        /// <returns></returns>
        public async Task TriggerEvent(EventTypeEnum eventType, CardRequestContext cardRequestContext, RoundContext roundContext, CardResponseContext cardResponseContext)
        {
            var eventDic = EventDic.ContainsKey(eventType) ? EventDic[eventType] : null;
            if (eventDic == null)
            {
                return;
            }

            foreach (var evtBind in eventDic)
            {
                foreach (var eventHandler in evtBind.Value.EventHandlers)
                {
                    if (eventHandler == null)
                    {
                        continue;
                    }
                    await eventHandler.RoundEventHandler(cardRequestContext, roundContext, cardResponseContext);
                    //log事件绑定成功
                    Console.WriteLine($"TriggerEvent:{eventHandler?.EventId}.");
                }
            }
        }

        /// <summary>
        /// 待移除的一次性事件监听id和对应的EventType
        /// </summary>
        private readonly ConcurrentDictionary<Guid, EventTypeEnum> PendingRemovalEventIdDic = new ConcurrentDictionary<Guid, EventTypeEnum>();

        /// <summary>
        ///  所有事件及其处理器
        /// ConcurrentDictionary<EventTypeEnum, ConcurrentDictionary<EventId, EventHandler>>
        /// </summary>
        private readonly ConcurrentDictionary<EventTypeEnum, ConcurrentDictionary<PlayerHero, EventData>> EventDic =
            new ConcurrentDictionary<EventTypeEnum, ConcurrentDictionary<PlayerHero, EventData>>();
    }

    public class EventData
    {
        public List<EventHandler> EventHandlers { get; set; }

    }

    public class EventHandler
    {
        public Guid EventId { get; set; }

        public EventBus.RoundEventHandler RoundEventHandler { get; set; }
    }
}
