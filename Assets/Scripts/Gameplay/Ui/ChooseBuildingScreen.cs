using System.Collections.Generic;
using UnityEngine;
using Domination.EventsSystem;
using UnityEngine.UI;
using Utils.Ui;


namespace Domination.Ui
{
    public class ChooseBuildingScreen : UiScreen
    {
        [SerializeField] private BuildingChoiceButton choiceButtonPrefab = default;
        [SerializeField] private Transform buttonsRoot = default;
        [SerializeField] private Button closeButton = default;

        private List<BuildingChoiceButton> createdButtons = new List<BuildingChoiceButton>();
        private int settlmentId;


        public override ScreenType Type => ScreenType.ChooseBuildingScreen;


        private void Awake()
        {
            EventsAggregator.TriggerEvent(new BuildOptionChosenMessage());
            closeButton.onClick.AddListener(Hide);
        }

        public void Show(int settlmentId)
        {
            base.Show();

            this.settlmentId = settlmentId;
            EventsAggregator.Subscribe(MessageType.UpdateUi, HandlePlayerSettlmentsUpdate);

            RefreshUi();
        }


        public override void Hide()
        {
            base.Hide();

            EventsAggregator.Unsubscribe(MessageType.UpdateUi, HandlePlayerSettlmentsUpdate);
        }


        private void HandlePlayerSettlmentsUpdate(IMessage _) => RefreshUi();


        private void RefreshUi()
        {
            foreach (var button in createdButtons)
            {
                Destroy(button.gameObject);
            }

            createdButtons.Clear();

            var availableBuildings = BuildingSystem.GetAvailableBuildings(settlmentId);
            foreach (var building in availableBuildings)
            {
                BuildingChoiceButton button = Instantiate(choiceButtonPrefab, buttonsRoot);
                button.Init(BuildingSystem.CanBuild(settlmentId, building), building, () =>
                {
                    EventsAggregator.TriggerEvent(new BuildOptionChosenMessage(building));
                    Hide();
                });
                button.gameObject.SetActive(true);
                createdButtons.Add(button);
            }
        }
    }
}
