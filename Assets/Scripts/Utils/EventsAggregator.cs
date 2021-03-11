using System;
using System.Collections.Generic;
using UnityEngine;


namespace Domination.EventsSystem
{
    public class EventsAggregator
    {
        private Dictionary<Type, HashSet<Action<IMessage>>> subscriptions = new Dictionary<Type, HashSet<Action<IMessage>>>();

        private EventsAggregator parent;
        private HashSet<EventsAggregator> children = new HashSet<EventsAggregator>();

        private bool isDisabled;


        public EventsAggregator() { }

        public EventsAggregator(EventsAggregator parent)
        {
            if (parent.isDisabled)
            {
                isDisabled = true;
            }
            else
            {
                this.parent = parent;
                parent.AddChild(this);
            }
        }

        public void Subscribe(Type eventType, Action<IMessage> handler)
        {
            if (isDisabled)
            {
                Debug.LogError("Trying to subscribe to a shutdown aggregator");
                return;
            }

            if (!subscriptions.ContainsKey(eventType))
            {
                subscriptions.Add(eventType, new HashSet<Action<IMessage>>());
            }

            subscriptions[eventType].Add(handler);
        }

        public void Unsubscribe(Type eventType, Action<IMessage> handler) //Keep it simple for now until I come across removal problems
        {
            if (subscriptions.ContainsKey(eventType))
            {
                subscriptions[eventType].Remove(handler);
            }
        }

        public void TriggerEvent(IMessage message)
        {
            if (isDisabled)
            {
                Debug.LogError("Trying to trigger event on a shutdown aggregator");
                return;
            }

            if (parent != null)
            {
                parent.TriggerEvent(message);
            }
            else
            {
                SendMessage(message);

                var childrenCopy = new HashSet<EventsAggregator>(children);

                foreach (var child in childrenCopy)
                {
                    child.SendMessage(message);
                }
            }
        }

        public void ShutDown()
        {
            isDisabled = true;
            subscriptions.Clear();

            var childrenToShutDown = new HashSet<EventsAggregator>(children);

            foreach (var child in childrenToShutDown)
            {
                child.ShutDown();
            }

            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

        private void AddChild(EventsAggregator child)
        {
            if (isDisabled)
            {
                Debug.LogError("Trying to add a child to a shutdown aggregator");
                return;
            }

            children.Add(child);
        }       
        
        private void RemoveChild(EventsAggregator child)
        {
            children.Remove(child);
        }

        private void SendMessage(IMessage message)
        {
            if (subscriptions.TryGetValue(message.GetType(), out var actions))
            {
                foreach (var action in actions)
                {
                    action(message);
                }
            }
        }
    } 
}
