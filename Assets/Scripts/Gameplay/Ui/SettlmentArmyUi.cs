using Domination.Warfare;
using System.Collections.Generic;
using UnityEngine;


namespace Domination.Ui
{
    public class SettlmentArmyUi : MonoBehaviour
    {
        [SerializeField] private UnitUi unitUiPrefab = default;
        [SerializeField] private Transform unitsRoot = default;

        private List<UnitUi> unitsUi = new List<UnitUi>();

        public void Init(List<Unit> army)
        {
            foreach (var unit in army)
            {
                UnitUi unitUi = Instantiate(unitUiPrefab, unitsRoot);
                unitUi.Init(unit);
                unitUi.gameObject.SetActive(true);
                unitsUi.Add(unitUi);
            }
        }
    }
}
