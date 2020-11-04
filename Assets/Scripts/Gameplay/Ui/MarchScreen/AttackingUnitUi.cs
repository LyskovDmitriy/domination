using Domination.Utils;
using Domination.Warfare;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Domination.Ui
{
    public class AttackingUnitUi : MonoBehaviour
    {
        private const string DAYS_TO_ARRIVE_FORMAT = "Arrives in {0} days";

        [SerializeField] private TextMeshProUGUI nameLabel = default;
        [SerializeField] private TextMeshProUGUI healthLabel = default;
        [SerializeField] private TextMeshProUGUI damageLabel = default;
        [SerializeField] private Button returnUnitButton = default;
        [SerializeField] private TextMeshProUGUI daysToArriveLabel = default;


        public void Init(Unit unit, Action<Unit> transferUnitAction, bool canBeReturned, int daysToArrive)
        {
            nameLabel.text = UnitsNameConstructor.Build(unit.Weapon);
            damageLabel.text = unit.Weapon.Damage.ToString();
            healthLabel.text = unit.Health.ToString();

            daysToArriveLabel.gameObject.SetActive(daysToArrive > 0);
            daysToArriveLabel.text = string.Format(DAYS_TO_ARRIVE_FORMAT, daysToArrive);

            if (canBeReturned)
            {
                returnUnitButton.onClick.AddListener(() => transferUnitAction.Invoke(unit));
            }
        }
    }
}
