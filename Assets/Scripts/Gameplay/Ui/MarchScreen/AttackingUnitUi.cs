using Domination.Utils;
using Domination.Warfare;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Domination.Ui.Marching
{
    public class AttackingUnitUi : MonoBehaviour
    {
        private const string DAYS_TO_ARRIVE_FORMAT = "Arrives in {0} days";

        [SerializeField] private TextMeshProUGUI nameLabel = default;
        [SerializeField] private TextMeshProUGUI healthLabel = default;
        [SerializeField] private TextMeshProUGUI damageLabel = default;
        [SerializeField] private Button returnUnitButton = default;
        [SerializeField] private TextMeshProUGUI daysToArriveLabel = default;


        public void Init(AttackingUnit unit, Action<AttackingUnit> transferUnitAction)
        {
            nameLabel.text = UnitsNameConstructor.Build(unit.Weapon);
            damageLabel.text = unit.Weapon.Damage.ToString();
            healthLabel.text = unit.Health.ToString();

            daysToArriveLabel.gameObject.SetActive(unit.MarchingTime > 0);
            daysToArriveLabel.text = string.Format(DAYS_TO_ARRIVE_FORMAT, unit.MarchingTime);

            returnUnitButton.gameObject.SetActive(unit.OriginalSettlment != null);
            returnUnitButton.onClick.RemoveAllListeners();

            if (unit.OriginalSettlment != null)
            {
                returnUnitButton.onClick.AddListener(() => transferUnitAction.Invoke(unit));
            }
        }
    }
}
