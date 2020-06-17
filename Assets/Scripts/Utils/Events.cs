using System;
using Utils.Ui;


namespace Domination.EventsSystem
{
    public enum MessageType
    {
        PlayerEndTurnRequest,
        PlayerSettlmentChanged,
        PlayerCoinsCountUpdate,
        RequestPlayerCoinsUpdate,
        UpdateUi,

        BuildOptionChosen,

        ShowUi,
        HideUi,
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

    public struct ShowUiMessage : IMessage
    {
        public MessageType Type => MessageType.ShowUi;
        public readonly ScreenType ScreenType;
        public readonly Action<UiScreen> OnInit;

        public ShowUiMessage(ScreenType screenType, Action<UiScreen> onInit = null)
        {
            ScreenType = screenType;
            OnInit = onInit;
        }
    }

    public struct HideUiMessage : IMessage
    {
        public MessageType Type => MessageType.HideUi;
        public readonly ScreenType ScreenType;

        public HideUiMessage(ScreenType screenType)
        {
            ScreenType = screenType;
        }
    }

    public struct BuildOptionChosenMessage : IMessage
    {
        public MessageType Type => MessageType.BuildOptionChosen;
        public readonly BuildingType BuildingType;


        public BuildOptionChosenMessage(BuildingType buildingType)
        {
            BuildingType = buildingType;
        }
    }
}
