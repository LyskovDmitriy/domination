using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Domination.Warfare;
using Domination.Utils;


namespace Domination.Ui
{
    public class UnitUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameLabel = default;
        [SerializeField] private TextMeshProUGUI healthLabel = default;
        [SerializeField] private TextMeshProUGUI damageLabel = default;


        public void Init(Unit unit)
        {
            nameLabel.text = UnitsNameConstructor.Build(unit.Weapon);
            damageLabel.text = unit.Weapon.Damage.ToString();
            healthLabel.text = unit.Health.ToString();
        }
    }
}
