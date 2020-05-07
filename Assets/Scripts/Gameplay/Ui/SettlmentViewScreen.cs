using System;
using System.Collections.Generic;
using UnityEngine;
using Domination.EventsSystem;


namespace Domination.Ui
{
    public class SettlmentViewScreen : UiUnit<object>
    {
        public static readonly ResourceBehavior<SettlmentViewScreen> Prefab = new ResourceBehavior<SettlmentViewScreen>("Ui/SettlmentViewScreen");

        [SerializeField] private BuiltSlotUI builtSlotPrefab = default;
        [SerializeField] private EmptySlotUi emptySlotUi = default;
        [SerializeField] private RectTransform slotsRoot = default;

        private List<GameObject> createdSlots = new List<GameObject>();

        private Settlment selectedSettlment;


        public void Show(Settlment settlment, Action<object> onHidden = null)
        {
            Show(onHidden);

            EventsAggregator.Subscribe(MessageType.UpdateUi, HandlePlayerSettlmentsUpdate);

            selectedSettlment = settlment;

            RefreshUi();
        }


        public override void Hide(object result)
        {
            base.Hide(result);

            EventsAggregator.Unsubscribe(MessageType.UpdateUi, HandlePlayerSettlmentsUpdate);
        }


        private void HandlePlayerSettlmentsUpdate(IMessage _) => RefreshUi();


        private void RefreshUi()
        {
            foreach (GameObject slot in createdSlots)
            {
                Destroy(slot);
            }

            createdSlots.Clear();

            List<Settlment.Building> buildings = selectedSettlment.GetBuildings();

            for (int i = 0; i < SettlmentsSettings.GetMaxBuildingsCount(selectedSettlment.Type); i++)
            {
                if (i < buildings.Count)
                {
                    BuiltSlotUI buildingInfo = Instantiate(builtSlotPrefab, slotsRoot);
                    buildingInfo.SetInfo(selectedSettlment.Id, selectedSettlment.Type, buildings[i]);
                    createdSlots.Add(buildingInfo.gameObject);
                }
                else
                {
                    EmptySlotUi emptySlot = Instantiate(emptySlotUi, slotsRoot);
                    createdSlots.Add(emptySlot.gameObject);
                    emptySlot.Init(() =>
                    {
                        ChooseBuildingScreen.Prefab.Instance.Show(selectedSettlment.Id, (chosenBuilding) =>
                        {
                            if (chosenBuilding != BuildingType.None)
                            {
                                BuildingSystem.Build(selectedSettlment.Id, chosenBuilding);
                            }
                        });
                    });
                }
            }
        }
    }
}
