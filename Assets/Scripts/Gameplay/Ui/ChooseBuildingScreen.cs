using System;
using System.Collections.Generic;
using UnityEngine;
using Domination.EventsSystem;
using UnityEngine.UI;


namespace Domination.Ui
{
    public class ChooseBuildingScreen : UiUnit<BuildingType>
    {
        public static readonly ResourceBehavior<ChooseBuildingScreen> Prefab = new ResourceBehavior<ChooseBuildingScreen>("Ui/ChooseBuildingScreen");

        [SerializeField] private BuildingChoiceButton choiceButtonPrefab = default;
        [SerializeField] private Transform buttonsRoot = default;
        [SerializeField] private Button closeButton = default;

        private List<BuildingChoiceButton> createdButtons = new List<BuildingChoiceButton>();
        private int settlmentId;


        private void Awake()
        {
            closeButton.onClick.AddListener(() => Hide(BuildingType.None));
        }

        public void Show(int settlmentId,  Action<BuildingType> onHidden = null)
        {
            base.Show(onHidden);

            this.settlmentId = settlmentId;
            EventsAggregator.Subscribe(MessageType.UpdateUi, HandlePlayerSettlmentsUpdate);

            RefreshUi();
        }


        public override void Hide(BuildingType result)
        {
            base.Hide(result);

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
                button.Init(BuildingSystem.CanBuild(settlmentId, building), building, () => Hide(building));
                button.gameObject.SetActive(true);
                createdButtons.Add(button);
            }
        }
    }
}
