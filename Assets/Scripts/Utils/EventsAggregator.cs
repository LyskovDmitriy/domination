using System;
using System.Collections.Generic;


namespace Domination.Events
{
    public static class EventsAggregator
    {
        private static readonly Dictionary<EventType, HashSet<Action<IEventMessage>>> Subscriptions = new Dictionary<EventType, HashSet<Action<IEventMessage>>>();

        public static void Subscribe(EventType eventType, Action<IEventMessage> handler)
        {
            if (!Subscriptions.ContainsKey(eventType))
            {
                Subscriptions.Add(eventType, new HashSet<Action<IEventMessage>>());
            }

            Subscriptions[eventType].Add(handler);
        }


        public static void Unsubscribe(EventType eventType, Action<IEventMessage> handler) //Keep it simple for now until I come across removal problems
        {
            if (Subscriptions.ContainsKey(eventType))
            {
                Subscriptions[eventType].Remove(handler);
            }
        }


        public static void TriggerEvent(EventType eventType, IEventMessage message)
        {
            if (Subscriptions.TryGetValue(eventType, out var actions))
            {
                foreach (var action in actions)
                {
                    action(message);
                }
            }
        }
    } 
}
