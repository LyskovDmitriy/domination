using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Domination.Ui
{
    public class BuiltSlotUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameLabel = default;
        [SerializeField] private Button upgradeButton = default;
        [SerializeField] private Button destroyButton = default;


        private void Awake()
        {
            upgradeButton.onClick.AddListener(UpgradeBuilding);
            destroyButton.onClick.AddListener(DestroyBuilding);
        }


        public void SetInfo(SettlmentType settlmentType, Settlment.Building buildingInfo)
        {
            nameLabel.text = $"{buildingInfo.type} {buildingInfo.level}";

            int maxBuildingLevel = SettlmentsSettings.GetMaxBuildingLevel(settlmentType, buildingInfo.type);
            upgradeButton.interactable = (buildingInfo.level < maxBuildingLevel);
        }


        private void UpgradeBuilding()
        {
        }


        private void DestroyBuilding()
        {
        }
    }
}
