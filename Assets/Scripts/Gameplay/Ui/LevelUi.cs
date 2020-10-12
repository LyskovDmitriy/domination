using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Domination.EventsSystem;
using Utils.Ui;


namespace Domination.Ui
{
    public class LevelUi : UiScreen
    {
        [SerializeField] private Button endTurnButton = default;
        [SerializeField] private TextMeshProUGUI playerMoneyLabel = default;

        public override ScreenType Type => ScreenType.LevelUi;


        private void Awake()
        {
            endTurnButton.onClick.AddListener(() => EventsAggregator.TriggerEvent(new RequestPlayerTurnEndMessage()));
        }


        public override void Show()
        {
            base.Show();

            EventsAggregator.Subscribe(typeof(PlayerCoinsCountUpdateMessage), OnPlayerCoinsCountChange);

            EventsAggregator.TriggerEvent(new RequestPlayerCoinsUpdateMessage());
        }


        public override void Hide()
        {
            base.Hide();

            EventsAggregator.Unsubscribe(typeof(PlayerCoinsCountUpdateMessage), OnPlayerCoinsCountChange);
        }


        private void OnPlayerCoinsCountChange(IMessage message)
        {
            PlayerCoinsCountUpdateMessage coinsChangeMessage = (PlayerCoinsCountUpdateMessage)message;
            playerMoneyLabel.text = coinsChangeMessage.Coins.ToString();
        }
    }
}
