using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Domination.Ui
{
    public class SettlmentViewScreen : UiUnit<object>
    {
        public static readonly ResourceBehavior<SettlmentViewScreen> Prefab = new ResourceBehavior<SettlmentViewScreen>("Ui/SettlmentViewScreen");

        [SerializeField] private BuiltSlotUI builtSlotPrefab = default;
        [SerializeField] private EmptySlotUi emptySlotUi = default;
        [SerializeField] private RectTransform slotsRoot = default;

        private List<GameObject> createdSlots = new List<GameObject>();


        public void Show(Settlment settlment, Action<object> onHidden = null)
        {
            Show(onHidden);

            foreach (GameObject slot in createdSlots)
            {
                Destroy(slot);
            }

            createdSlots.Clear();

            List<Settlment.Building> buildings = settlment.Buildings;

            for (int i = 0; i < SettlmentsSettings.GetMaxBuildingsCount(settlment.Type); i++)
            {
                if (i < buildings.Count)
                {
                    BuiltSlotUI buildingInfo = Instantiate(builtSlotPrefab, slotsRoot);
                    buildingInfo.SetInfo(settlment.Type, buildings[i]);
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
