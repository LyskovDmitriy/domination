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
            endTurnButton.onClick.AddListener(() => Aggregator.TriggerEvent(new RequestPlayerTurnEndMessage()));
        }


        public override void Show()
        {
            base.Show();

            Aggregator.Subscribe(typeof(PlayerCoinsCountUpdateMessage), OnPlayerCoinsCountChange);

            Aggregator.TriggerEvent(new RequestPlayerCoinsUpdateMessage());
        }


        public override void Hide()
        {
            base.Hide();

            Aggregator.Unsubscribe(typeof(PlayerCoinsCountUpdateMessage), OnPlayerCoinsCountChange);
        }


        private void OnPlayerCoinsCountChange(IMessage message)
        {
            PlayerCoinsCountUpdateMessage coinsChangeMessage = (PlayerCoinsCountUpdateMessage)message;
            playerMoneyLabel.text = coinsChangeMessage.Coins.ToString();
        }
    }
}
