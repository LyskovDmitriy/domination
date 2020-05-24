using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Domination.EventsSystem;


namespace Domination.Ui
{
    public class LevelUi : UiUnit<object>
    {
        public static readonly ResourceBehavior<LevelUi> Prefab = new ResourceBehavior<LevelUi>("Ui/LevelUi");

        [SerializeField] private Button endTurnButton = default;
        [SerializeField] private TextMeshProUGUI playerMoneyLabel = default;


        private void Awake()
        {
            endTurnButton.onClick.AddListener(() => EventsAggregator.TriggerEvent(new RequestPlayerTurnEndMessage()));
        }


        public override void Show(Action<object> onHidden = null)
        {
            base.Show(onHidden);

            EventsAggregator.Subscribe(MessageType.PlayerCoinsCountUpdate, OnPlayerCoinsCountChange);

            EventsAggregator.TriggerEvent(new RequestPlayerCoinsUpdateMessage());
        }


        public override void Hide(object result)
        {
            base.Hide(result);

            EventsAggregator.Unsubscribe(MessageType.PlayerCoinsCountUpdate, OnPlayerCoinsCountChange);
        }


        private void OnPlayerCoinsCountChange(IMessage message)
        {
            PlayerCoinsCountUpdateMessage coinsChangeMessage = (PlayerCoinsCountUpdateMessage)message;
            playerMoneyLabel.text = coinsChangeMessage.Coins.ToString();
        }
    }
}
