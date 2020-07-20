using Domination.Warfare;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils.Ui;


namespace Domination.Ui
{
    public class MarchScreen : UiScreen
    {
        private class SettlmentArmy
        {
            public Settlment settlment;
            public SettlmentArmyUi armyUi;
            public Army army;
        }


        [SerializeField] private SettlmentArmyUi settlmentArmyPrefab = default;
        [SerializeField] private RectTransform settlmentArmiesRoot = default;
        [SerializeField] private Button closeButton = default;
        [SerializeField] private SettlmentArmyUi marchingArmy = default;
        [SerializeField] private Button sendArmyButton = default;

        private List<SettlmentArmy> availableArmies = new List<SettlmentArmy>();
        private Settlment attackedSettlment;
        private Army attackingArmy;


        public override ScreenType Type => ScreenType.MarchScreen;


        private void Awake()
        {
            closeButton.onClick.AddListener(Hide);
            sendArmyButton.onClick.AddListener(SendArmy);
        }


        public void Show(Character player, Settlment targetSettlment)
        {
            base.Show();

            foreach (var army in availableArmies)
            {
                Destroy(army.armyUi.gameObject);
            }
            availableArmies.Clear();

            attackedSettlment = targetSettlment;

            foreach (var settlment in player.Settlments)
            {
                SettlmentArmyUi settlmentArmyUi = Instantiate(settlmentArmyPrefab, settlmentArmiesRoot);
                settlmentArmyUi.gameObject.SetActive(true);
                availableArmies.Add(new SettlmentArmy { settlment = settlment, armyUi = settlmentArmyUi, army = new Army(player.GetSettlmentArmy(settlment)) });
            }

            attackingArmy = new Army();
            RefreshArmiesUi();
        }

        private void RefreshArmiesUi()
        {
            foreach (var availableArmy in availableArmies)
            {
                availableArmy.armyUi.SetInfo(availableArmy.settlment.Title, availableArmy.army, (unit) => SendUnitInRaid(availableArmy, unit));
            }

            marchingArmy.SetInfo(attackedSettlment.Title, attackingArmy, ReturnUnitToSettlment);
            sendArmyButton.enabled = (attackingArmy.TotalUnitsCount > 0);
        }

        private void SendUnitInRaid(SettlmentArmy settlmentArmy, Unit unit)
        {
            settlmentArmy.army.RemoveUnit(unit);
            attackingArmy.AddUnit(new AttackingUnit(unit, settlmentArmy.settlment));
            RefreshArmiesUi();
        }

        private void ReturnUnitToSettlment(Unit unit)
        {
            AttackingUnit attackingUnit = (AttackingUnit)unit;
            SettlmentArmy settlmentArmy = availableArmies.Find((a) => a.settlment == attackingUnit.originalSettlment);
            attackingArmy.RemoveUnit(attackingUnit);
            settlmentArmy.army.AddUnit(new Unit(attackingUnit));
            RefreshArmiesUi();
        }

        private void SendArmy()
        { 
        }
    }
}
