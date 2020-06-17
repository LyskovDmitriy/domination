using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


namespace Domination.Ui
{
    public class BuildingChoiceButton : MonoBehaviour
    {
        [SerializeField] private Button button = default;
        [SerializeField] private TextMeshProUGUI titleLabel = default;
        [SerializeField] private TextMeshProUGUI priceLabel = default;


        public void Init(bool isInteractable, BuildingType buildingType, UnityAction onClick)
        {
            titleLabel.text = buildingType.ToString();
            priceLabel.text = BuildingSystem.GetConstructionPrice(buildingType).ToString();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClick);
            button.interactable = isInteractable;
        }
    }
}
