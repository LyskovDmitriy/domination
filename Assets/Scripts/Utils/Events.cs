using System;
using Utils.Ui;


namespace Domination.EventsSystem
{
    public interface IMessage { }

    public struct BuildOptionChosenMessage : IMessage
    {
        public readonly BuildingType BuildingType;

        public BuildOptionChosenMessage(BuildingType buildingType)
        {
            BuildingType = buildingType;
        }
    }

    public struct HideUiMessage : IMessage
    {
        public readonly ScreenType ScreenType;

        public HideUiMessage(ScreenType screenType)
        {
            ScreenType = screenType;
        }
    }

    public struct PlayerSettlmentChangedMessage : IMessage { }

    public struct PlayerCoinsCountUpdateMessage : IMessage
    {
        public readonly int Coins;

        public PlayerCoinsCountUpdateMessage(int coins)
        {
            Coins = coins;
        }
    }

    public struct RequestPlayerTurnEndMessage : IMessage { }

    public struct RequestPlayerCoinsUpdateMessage : IMessage { }

    public struct ShowUiMessage : IMessage
    {
        public readonly ScreenType ScreenType;
        public readonly Action<UiScreen> OnInit;

        public ShowUiMessage(ScreenType screenType, Action<UiScreen> onInit = null)
        {
            ScreenType = screenType;
            OnInit = onInit;
        }
    }

    public struct UpdateUiMessage : IMessage { }

    public struct UnitRecruitedMessage : IMessage
    {
        public readonly uint CharacterId;
        public readonly uint SettlmentId;

        public UnitRecruitedMessage(uint characterId, uint settlmentId)
        {
            CharacterId = characterId;
            SettlmentId = settlmentId;
        }
    }
}
