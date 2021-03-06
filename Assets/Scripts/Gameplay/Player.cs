using System;
using Domination.Data;
using Domination.EventsSystem;
using Domination.LevelLogic;


namespace Domination
{
    public class Player : Character
    {
        public Player() : base()
        {
            SubscribeToMessages();
        }

        public Player(Func<uint, Settlment> settlmentGetter, CharacterData data) : 
            base(settlmentGetter, data) 
        {
            SubscribeToMessages();
        }

        ~Player()
        {
            EventsAggregator.Unsubscribe(typeof(RequestPlayerTurnEndMessage), HandleTurnEndRequest);
            EventsAggregator.Unsubscribe(typeof(RequestPlayerCoinsUpdateMessage), HandleCoinsUpdateRequest);
        }


        protected override void SetNewCoinsCount(int coins)
        {
            base.SetNewCoinsCount(coins);

            SendCoinsUpdateMessage();
        }

        public override void UpgradeBuilding(uint settlmentId, BuildingType buildingType)
        {
            base.UpgradeBuilding(settlmentId, buildingType);

            EventsAggregator.TriggerEvent(new PlayerSettlmentChangedMessage());
        }

        public override void DestroyBuilding(uint settlmentId, BuildingType buildingType)
        {
            base.DestroyBuilding(settlmentId, buildingType);

            EventsAggregator.TriggerEvent(new PlayerSettlmentChangedMessage());
        }

        public override void Build(uint settlmentId, BuildingType buildingType)
        {
            base.Build(settlmentId, buildingType);

            EventsAggregator.TriggerEvent(new PlayerSettlmentChangedMessage());
        }

        private void SendCoinsUpdateMessage() => EventsAggregator.TriggerEvent(new PlayerCoinsCountUpdateMessage(Coins));

        private void HandleTurnEndRequest(IMessage _) => FinishTurn();

        private void HandleCoinsUpdateRequest(IMessage _) => SendCoinsUpdateMessage();

        private void SubscribeToMessages()
        {
            EventsAggregator.Subscribe(typeof(RequestPlayerTurnEndMessage), HandleTurnEndRequest);
            EventsAggregator.Subscribe(typeof(RequestPlayerCoinsUpdateMessage), HandleCoinsUpdateRequest);
        }
    }
}
