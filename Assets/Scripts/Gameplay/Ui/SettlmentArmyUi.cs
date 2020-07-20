using Domination.Warfare;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Domination.Ui
{
    public class SettlmentArmyUi : MonoBehaviour
    {
        [SerializeField] private UnitUi unitUiPrefab = default;
        [SerializeField] private RectTransform unitsRoot = default;
        [SerializeField] private TextMeshProUGUI settlmentTitleLabel = default;

        private List<UnitUi> unitsUi = new List<UnitUi>();

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
                UnitUi unitUi = Instantiate(unitUiPrefab, unitsRoot);
                unitUi.Init(unit, transferUnitAction);
                unitUi.gameObject.SetActive(true);
                unitsUi.Add(unitUi);
            }
        }
    }
}
