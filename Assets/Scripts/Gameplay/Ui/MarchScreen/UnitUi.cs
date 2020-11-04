using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Domination.Warfare;
using Domination.Utils;


namespace Domination.Ui.Marching
{
    public class UnitUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameLabel = default;
        [SerializeField] private TextMeshProUGUI healthLabel = default;
        [SerializeField] private TextMeshProUGUI damageLabel = default;
        [SerializeField] private Button transferUnitButton = default;


        public void Init(Unit unit, Action<Unit> transferUnitAction)
        {
            nameLabel.text = UnitsNameConstructor.Build(unit.Weapon);
            damageLabel.text = unit.Weapon.Damage.ToString();
            healthLabel.text = unit.Health.ToString();

            transferUnitButton.onClick.AddListener(() => transferUnitAction?.Invoke(unit));
        }
    }
}
