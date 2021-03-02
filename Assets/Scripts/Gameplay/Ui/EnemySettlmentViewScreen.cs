using UnityEngine;
using Domination.EventsSystem;
using UnityEngine.UI;
using Utils.Ui;
using Domination.Ui.Marching;
using Domination.LevelLogic;


namespace Domination.Ui
{
    public class EnemySettlmentViewScreen : UiScreen
    {
        [SerializeField] private Button sendTroopsButton = default;
        [SerializeField] private Button attackSettlmentButton = default;

        private Settlment selectedSettlment;
        private Character player;
        private Level level;


        public override ScreenType Type => ScreenType.EnemySettlmentViewScreen;


        private void Awake()
        {
            sendTroopsButton.onClick.AddListener(OpenMarchScreen);
            attackSettlmentButton.onClick.AddListener(AttackSettlment);
        }


        public void Show(Settlment settlment, Level level)
        {
            Show();

            selectedSettlment = settlment;
            this.level = level;
            player = level.Player;

            EventsAggregator.Subscribe(typeof(UpdateUiMessage), HandleSettlmentsUpdate);
            attackSettlmentButton.interactable = player.HasUnitsInSettlment(selectedSettlment);

            RefreshUi();
        }


        public override void Hide()
        {
            base.Hide();

            EventsAggregator.Unsubscribe(typeof(UpdateUiMessage), HandleSettlmentsUpdate);

            selectedSettlment = null;
        }

        private void HandleSettlmentsUpdate(IMessage _) => RefreshUi();

        private void RefreshUi()
        {
            
        }

        private void OpenMarchScreen()
        {
            EventsAggregator.TriggerEvent(new ShowUiMessage(ScreenType.MarchScreen, (screen) =>
            {
                var marchScreen = (MarchScreen)screen;
                marchScreen.Show(level, selectedSettlment);
            }));
        }

        private void AttackSettlment()
        {
            EventsAggregator.TriggerEvent(new PlayerAttackSettlment(selectedSettlment.Id));
        }
    }
}
