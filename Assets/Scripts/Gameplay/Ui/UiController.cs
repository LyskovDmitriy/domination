using UnityEngine;
using Domination.EventsSystem;


namespace Domination.Ui
{
    public static class UiController
    {
        private static readonly MessageType[] MessagesToUpdateUi = new MessageType[]
            {
                MessageType.PlayerSettlmentChanged
            };

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            foreach (var messageType in MessagesToUpdateUi)
            {
                EventsAggregator.Subscribe(messageType, SendUpdateUiMessage);
            }
        }


        public static void SendUpdateUiMessage(IMessage message) => EventsAggregator.TriggerEvent(new UpdateUiMessage());
    }
}
