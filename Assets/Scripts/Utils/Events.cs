using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Domination.EventsSystem
{
    public enum MessageType
    {
        DestroyBuilding,
        PlayerEndTurnRequest,
        PlayerSettlmentChanged
    }


    public interface IMessage
    {
        MessageType Type { get; }
    }


    public struct DestroyBuildingMessage : IMessage
    {
        public MessageType Type => MessageType.DestroyBuilding;

        public readonly int SettlmentId;

        public readonly BuildingType BuildingType;

        public DestroyBuildingMessage(int settlmentId, BuildingType buildingType)
        {
            SettlmentId = settlmentId;
            BuildingType = buildingType;
        }
    }

    public struct PlayerSettlmentChangedMessage : IMessage
    {
        public MessageType Type => MessageType.PlayerSettlmentChanged;
    }

    public struct RequestPlayerTurnEndMessage : IMessage
    {
        public MessageType Type => MessageType.PlayerEndTurnRequest;
    }
}
