using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Domination.EventsSystem;


namespace Domination.Ui
{
    public class BuiltSlotUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameLabel = default;
        [SerializeField] private Button upgradeButton = default;
        [SerializeField] private Button destroyButton = default;


        public void SetInfo(int settlmentId, SettlmentType settlmentType, Settlment.Building buildingInfo)
        {
            nameLabel.text = $"{buildingInfo.type} {buildingInfo.level}";

            int maxBuildingLevel = SettlmentsSettings.GetMaxBuildingLevel(settlmentType, buildingInfo.type);
            upgradeButton.interactable = (buildingInfo.level < maxBuildingLevel);

            destroyButton.onClick.RemoveAllListeners();
            destroyButton.onClick.AddListener(() => DestroyBuilding(settlmentId, buildingInfo.type));
        }


        private void UpgradeBuilding(int settlementId, BuildingType building)
        {
        }


        private void DestroyBuilding(int settlementId, BuildingType building)
        {
            EventsAggregator.TriggerEvent(new DestroyBuildingMessage(settlementId, building));
        }
    }
}
