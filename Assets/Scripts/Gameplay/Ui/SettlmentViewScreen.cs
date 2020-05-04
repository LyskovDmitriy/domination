using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

            EventsAggregator.Subscribe(MessageType.PlayerSettlmentChanged, HandlePlayerSettlmentsUpdate);

            selectedSettlment = settlment;
            RefreshUi();
        }


        public override void Hide(object result)
        {
            base.Hide(result);

            EventsAggregator.Unsubscribe(MessageType.PlayerSettlmentChanged, HandlePlayerSettlmentsUpdate);
        }


        private void HandlePlayerSettlmentsUpdate(IMessage _) => RefreshUi();


        private void RefreshUi()
        {
            foreach (GameObject slot in createdSlots)
            {
                Destroy(slot);
            }

            createdSlots.Clear();

            List<Settlment.Building> buildings = selectedSettlment.Buildings;

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
                }
            }
        }
    }
}
