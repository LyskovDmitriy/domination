using System;
using System.Collections.Generic;


namespace Domination.EventsSystem
{
    public static class EventsAggregator
    {
        private static readonly Dictionary<Type, HashSet<Action<IMessage>>> Subscriptions = new Dictionary<Type, HashSet<Action<IMessage>>>();

        public static void Subscribe(Type eventType, Action<IMessage> handler)
        {
            if (!Subscriptions.ContainsKey(eventType))
            {
                Subscriptions.Add(eventType, new HashSet<Action<IMessage>>());
            }

            Subscriptions[eventType].Add(handler);
        }


        public static void Unsubscribe(Type eventType, Action<IMessage> handler) //Keep it simple for now until I come across removal problems
        {
            if (Subscriptions.ContainsKey(eventType))
            {
                Subscriptions[eventType].Remove(handler);
            }
        }


        public static void TriggerEvent(IMessage message)
        {
            if (Subscriptions.TryGetValue(message.GetType(), out var actions))
            {
                foreach (var action in actions)
                {
                    action(message);
                }
            }
        }
    } 
}
