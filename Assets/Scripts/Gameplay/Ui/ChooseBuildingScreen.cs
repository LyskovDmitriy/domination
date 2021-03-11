using System.Collections.Generic;
using UnityEngine;
using Domination.EventsSystem;
using Domination.LevelView;
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
        private uint settlmentId;


        public override ScreenType Type => ScreenType.ChooseBuildingScreen;


        private void Awake()
        {
            closeButton.onClick.AddListener(Hide);
        }

        public void Show(uint settlmentId)
        {
            base.Show();

            this.settlmentId = settlmentId;
            Aggregator.Subscribe(typeof(UpdateUiMessage), HandlePlayerSettlmentsUpdate);

            RefreshUi();
        }

        public override void Hide()
        {
            base.Hide();

            Aggregator.Unsubscribe(typeof(UpdateUiMessage), HandlePlayerSettlmentsUpdate);
        }

        private void HandlePlayerSettlmentsUpdate(IMessage _) => RefreshUi();

        private void RefreshUi()
        {
            foreach (var button in createdButtons)
            {
                Destroy(button.gameObject);
            }

            createdButtons.Clear();

            var availableBuildings = MapView.Instance.BuildingSystem.GetAvailableBuildings(settlmentId);
            foreach (var building in availableBuildings)
            {
                BuildingChoiceButton button = Instantiate(choiceButtonPrefab, buttonsRoot);
                button.Init(MapView.Instance.BuildingSystem.CanBuild(settlmentId, building), building, () =>
                {
                    Aggregator.TriggerEvent(new BuildOptionChosenMessage(building));
                    Hide();
                });
                button.gameObject.SetActive(true);
                createdButtons.Add(button);
            }
        }
    }
}
