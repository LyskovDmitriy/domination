using Domination.EventsSystem;


namespace Domination
{
    public class Player : Character
    {
        public Player(Castle castle) : base(castle)
        {
            PlayerId = Id;

            EventsAggregator.Subscribe(MessageType.PlayerEndTurnRequest, HandleTurnEndRequest);
            EventsAggregator.Subscribe(MessageType.RequestPlayerCoinsUpdate, HandleCoinsUpdateRequest);
        }

        ~Player()
        {
            EventsAggregator.Unsubscribe(MessageType.PlayerEndTurnRequest, HandleTurnEndRequest);
            EventsAggregator.Unsubscribe(MessageType.RequestPlayerCoinsUpdate, HandleCoinsUpdateRequest);
        }


        protected override void SetNewCoinsCount(int coins)
        {
            base.SetNewCoinsCount(coins);

            SendCoinsUpdateMessage();
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


        private void SendCoinsUpdateMessage() => EventsAggregator.TriggerEvent(new PlayerCoinsCountUpdateMessage(Coins));


        private void HandleTurnEndRequest(IMessage _) => FinishTurn();


        private void HandleCoinsUpdateRequest(IMessage _) => SendCoinsUpdateMessage();
    }
}
