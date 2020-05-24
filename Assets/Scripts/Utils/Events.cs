namespace Domination.EventsSystem
{
    public enum MessageType
    {
        PlayerEndTurnRequest,
        PlayerSettlmentChanged,
        PlayerCoinsCountUpdate,
        RequestPlayerCoinsUpdate,
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

    public struct PlayerCoinsCountUpdateMessage : IMessage
    {
        public MessageType Type => MessageType.PlayerCoinsCountUpdate;
        public readonly int Coins;

        public PlayerCoinsCountUpdateMessage(int coins)
        {
            Coins = coins;
        }
    }

    public struct RequestPlayerCoinsUpdateMessage : IMessage
    {
        public MessageType Type => MessageType.RequestPlayerCoinsUpdate;
    }
}
