using System;
using System.Collections.Generic;
using UnityEngine;
using Domination.EventsSystem;


public class Player : Character
{
    public static event Action<int> OnCoinsCountChange;

    
    public Player()
    {
        EventsAggregator.Subscribe(MessageType.PlayerEndTurnRequest, (_) => FinishTurn());
        EventsAggregator.Subscribe(MessageType.DestroyBuilding, DestroyBuilding);
    }


    public override void Init(Castle castle)
    {
        base.Init(castle);
    }


    protected override void SetNewCoinsCount(int money)
    {
        base.SetNewCoinsCount(money);

        OnCoinsCountChange?.Invoke(money);
    }


    private void DestroyBuilding(IMessage message)
    {
        DestroyBuildingMessage destroyBuildingMessage = (DestroyBuildingMessage)message;

        Settlment settlment = settlments.Find((s) => s.Id == destroyBuildingMessage.SettlmentId);
        settlment.DestroyBuilding(destroyBuildingMessage.BuildingType);

        EventsAggregator.TriggerEvent(new PlayerSettlmentChangedMessage());
    }
}
