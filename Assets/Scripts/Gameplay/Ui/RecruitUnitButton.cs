using Domination.Warfare;
using Domination.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


namespace Domination.Ui
{
    public class RecruitUnitButton : MonoBehaviour
    {
        [SerializeField] private Button button = default;
        [SerializeField] private TextMeshProUGUI titleLabel = default;
        [SerializeField] private TextMeshProUGUI priceLabel = default;
        [SerializeField] private TextMeshProUGUI damageLabel = default;
        [SerializeField] private TextMeshProUGUI healthLabel = default;


        public void Init(UnityAction clickAction, bool canRecruit, WeaponType weaponType, int level)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(clickAction);
            button.interactable = canRecruit;

            WeaponInfo weaponInfo = UnitRecruitmentSettings.GetWeapons(weaponType)[level];
            titleLabel.text = UnitsNameConstructor.Build(weaponInfo.Weapon);
            damageLabel.text = weaponInfo.Weapon.Damage.ToString();
            healthLabel.text = RecruitmentSystem.GetHealth(weaponType).ToString();
            priceLabel.text = RecruitmentSystem.UnitPrice.ToString();
        }
    }
}
