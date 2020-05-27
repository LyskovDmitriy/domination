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


        public void Init(int settlmentId, SettlmentType settlmentType, Settlment.Building buildingInfo, bool isInteractable)
        {
            int maxBuildingLevel = SettlmentsSettings.GetMaxBuildingLevel(settlmentType, buildingInfo.type);

            upgradeButton.gameObject.SetActive(false);
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => BuildingSystem.UpgradeBuilding(settlmentId, buildingInfo.type));
            upgradeButton.interactable = isInteractable;

            switch (BuildingSystem.CanUpdateBuilding(Character.PlayerId, settlmentType, buildingInfo.type, buildingInfo.level))
            {
                case UpdatePossibility.AlreadyHighestLevel:
                    nameLabel.text = $"{buildingInfo.type} {buildingInfo.level}";
                    break;

                case UpdatePossibility.NotEnoughMoney:
                    upgradeButton.gameObject.SetActive(true);
                    upgradeButton.interactable = false;
                    nameLabel.text = $"{ buildingInfo.type } { buildingInfo.level }\nUp { BuildingSystem.GetUpgradePrice(buildingInfo.type, buildingInfo.level) }";
                    break;

                case UpdatePossibility.Possible:
                    upgradeButton.gameObject.SetActive(true);
                    upgradeButton.interactable = true;
                    nameLabel.text = $"{ buildingInfo.type } { buildingInfo.level }\nUp { BuildingSystem.GetUpgradePrice(buildingInfo.type, buildingInfo.level) }";
                    break;
            }

            destroyButton.onClick.RemoveAllListeners();
            destroyButton.onClick.AddListener(() => BuildingSystem.DestroyBuilding(settlmentId, buildingInfo.type));
        }
    }
}
