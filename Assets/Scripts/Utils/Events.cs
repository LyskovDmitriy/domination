using System.Collections.Generic;


namespace Domination.EventsSystem
{
    public enum MessageType
    {
        PlayerEndTurnRequest,
        PlayerSettlmentChanged,
        UpdateUi,
    }


    public interface IMessage
    {
        MessageType Type { get; }
    }

    public struct PlayerSettlmentChangedMessage : IMessage
    {
        public MessageType Type => MessageType.PlayerSettlmentChanged;
    }

    public struct RequestPlayerTurnEndMessage : IMessage
    {
        public MessageType Type => MessageType.PlayerEndTurnRequest;
    }

    public struct UpdateUiMessage : IMessage
    {
        public MessageType Type => MessageType.UpdateUi;
    }
}
