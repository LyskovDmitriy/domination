using System;
using System.Collections.Generic;
using UnityEngine;
using Domination.EventsSystem;


public class Player : Character
{
    public static event Action<int> OnCoinsCountChange;


    public override void Init(Castle castle)
    {
        base.Init(castle);

        PlayerId = Id;

        EventsAggregator.Subscribe(MessageType.PlayerEndTurnRequest, (_) => FinishTurn());
    }


    protected override void SetNewCoinsCount(int money)
    {
        base.SetNewCoinsCount(money);

        OnCoinsCountChange?.Invoke(money);
    }


    public override void UpgradeBuilding(int settlmentId, BuildingType buildingType)
    {
        base.UpgradeBuilding(settlmentId, buildingType);

        EventsAggregator.TriggerEvent(new PlayerSettlmentChangedMessage());
    }


    public override void DestroyBuilding(int settlmentId, BuildingType buildingType)
    {
        base.DestroyBuilding(settlmentId, buildingType);

        EventsAggregator.TriggerEvent(new PlayerSettlmentChangedMessage());
    }


    public override void Build(int settlmentId, BuildingType buildingType)
    {
        base.Build(settlmentId, buildingType);

        EventsAggregator.TriggerEvent(new PlayerSettlmentChangedMessage());
    }
}
