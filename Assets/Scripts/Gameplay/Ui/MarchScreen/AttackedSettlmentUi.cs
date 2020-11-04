using Domination.Warfare;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Domination.Ui.Marching
{
    public class AttackedSettlmentUi : MonoBehaviour
    {
        [SerializeField] private AttackingUnitUi unitUiPrefab = default;
        [SerializeField] private RectTransform unitsRoot = default;
        [SerializeField] private TextMeshProUGUI settlmentTitleLabel = default;

        private List<AttackingUnitUi> unitsUi = new List<AttackingUnitUi>();

        public void SetInfo(string settlmentTitle, AttackingArmy army, Action<AttackingUnit> transferUnitAction)
        {
            foreach (var unitUi in unitsUi)
            {
                Destroy(unitUi.gameObject);
            }

            unitsUi.Clear();

            settlmentTitleLabel.text = settlmentTitle;

            foreach (var unit in army.GetUnits())
            {
                AttackingUnitUi unitUi = Instantiate(unitUiPrefab, unitsRoot);
                unitUi.Init(unit, transferUnitAction);
                unitsUi.Add(unitUi);
            }
        }
    }
}
