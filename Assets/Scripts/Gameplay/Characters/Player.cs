using System;
using Domination.Data;
using Domination.EventsSystem;
using Domination.LevelLogic;


namespace Domination
{
    public class Player : Character
    {
        public Player(EventsAggregator aggregator) : base(aggregator)
        {
            SubscribeToMessages();
        }

        public Player(EventsAggregator aggregator, Func<uint, Settlment> settlmentGetter, CharacterData data) : 
            base(aggregator, settlmentGetter, data) 
        {
            SubscribeToMessages();
        }

        ~Player()
        {
            aggregator.Unsubscribe(typeof(RequestPlayerTurnEndMessage), HandleTurnEndRequest);
            aggregator.Unsubscribe(typeof(RequestPlayerCoinsUpdateMessage), HandleCoinsUpdateRequest);
        }


        protected override void SetNewCoinsCount(int coins)
        {
            base.SetNewCoinsCount(coins);

            SendCoinsUpdateMessage();
        }

        public override void UpgradeBuilding(uint settlmentId, BuildingType buildingType)
        {
            base.UpgradeBuilding(settlmentId, buildingType);

            aggregator.TriggerEvent(new PlayerSettlmentChangedMessage());
        }

        public override void DestroyBuilding(uint settlmentId, BuildingType buildingType)
        {
            base.DestroyBuilding(settlmentId, buildingType);

            aggregator.TriggerEvent(new PlayerSettlmentChangedMessage());
        }

        public override void Build(uint settlmentId, BuildingType buildingType)
        {
            base.Build(settlmentId, buildingType);

            aggregator.TriggerEvent(new PlayerSettlmentChangedMessage());
        }

        private void SendCoinsUpdateMessage() => aggregator.TriggerEvent(new PlayerCoinsCountUpdateMessage(Coins));

        private void HandleTurnEndRequest(IMessage _) => FinishTurn();

        private void HandleCoinsUpdateRequest(IMessage _) => SendCoinsUpdateMessage();

        private void SubscribeToMessages()
        {
            aggregator.Subscribe(typeof(RequestPlayerTurnEndMessage), HandleTurnEndRequest);
            aggregator.Subscribe(typeof(RequestPlayerCoinsUpdateMessage), HandleCoinsUpdateRequest);
        }
    }
}
