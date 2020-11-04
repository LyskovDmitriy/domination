using Domination.Warfare;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Domination.Ui
{
    public class AttackedSettlmentUi : MonoBehaviour
    {
        [SerializeField] private AttackingUnitUi unitUiPrefab = default;
        [SerializeField] private RectTransform unitsRoot = default;
        [SerializeField] private TextMeshProUGUI settlmentTitleLabel = default;

        private List<AttackingUnitUi> unitsUi = new List<AttackingUnitUi>();

        public void SetInfo(string settlmentTitle, Army army, Action<Unit> transferUnitAction)
        {
            foreach (var unitUi in unitsUi)
            {
                Destroy(unitUi.gameObject);
            }

            unitsUi.Clear();

            settlmentTitleLabel.text = settlmentTitle;
            List<Unit> units = army.GetUnits();

            foreach (var unit in units)
            {
                AttackingUnitUi unitUi = Instantiate(unitUiPrefab, unitsRoot);
                AttackingUnit attackingUnit = (AttackingUnit)unit;
                unitUi.Init(unit, transferUnitAction, true, attackingUnit.MarchingTime);
                unitsUi.Add(unitUi);
            }
        }
    }
}
