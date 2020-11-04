using Domination.Warfare;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Ui;


namespace Domination.Ui.Marching
{
    public class MarchScreen : UiScreen
    {
        private class SettlmentArmy
        {
            public Settlment settlment;
            public SettlmentArmyUi armyUi;
            public Army army;
            public int movementTime;
        }

        [SerializeField] private SettlmentArmyUi settlmentArmyPrefab = default;
        [SerializeField] private RectTransform settlmentArmiesRoot = default;
        [SerializeField] private Button closeButton = default;
        [SerializeField] private AttackedSettlmentUi attackedSettlmentUi = default;
        [SerializeField] private Button sendArmyButton = default;

        private List<SettlmentArmy> availableArmies = new List<SettlmentArmy>();
        private Settlment attackedSettlment;
        private AttackingArmy attackingArmy;

        private Level level;


        public override ScreenType Type => ScreenType.MarchScreen;


        private void Awake()
        {
            closeButton.onClick.AddListener(Hide);
            sendArmyButton.onClick.AddListener(SendArmy);
        }


        public void Show(Level level, Settlment targetSettlment)
        {
            base.Show();
            //Show all marching units

            this.level = level;

            foreach (var army in availableArmies)
            {
                Destroy(army.armyUi.gameObject);
            }
            availableArmies.Clear();

            attackedSettlment = targetSettlment;

            foreach (var settlment in level.Player.Settlments)
            {
                SettlmentArmyUi settlmentArmyUi = Instantiate(settlmentArmyPrefab, settlmentArmiesRoot);

                float distance = Pathfinding.GetDistance(settlment.Tile.Position, targetSettlment.Tile.Position, level.Map, TilesPassingCostContainer.GetTilePassingCost);
                int daysToArrive = Mathf.CeilToInt(distance / Constants.UNIT_MOVESPEED);

                availableArmies.Add(new SettlmentArmy 
                { 
                    settlment = settlment, 
                    armyUi = settlmentArmyUi, 
                    army = new Army(level.Player.GetSettlmentArmy(settlment)),
                    movementTime = daysToArrive
                });
            }

            CreateAttackingArmy();
            RefreshArmiesUi();
        }

        private void RefreshArmiesUi()
        {
            foreach (var availableArmy in availableArmies)
            {
                availableArmy.armyUi.SetInfo(
                    $"{availableArmy.settlment.Title} ({availableArmy.movementTime} days)", 
                    availableArmy.army, 
                    (unit) => SendUnitInRaid(availableArmy, unit, availableArmy.movementTime));
            }

            attackedSettlmentUi.SetInfo(attackedSettlment.Title, attackingArmy, ReturnUnitToSettlment);
            sendArmyButton.enabled = (attackingArmy.UnitsCount > 0);
        }

        private void SendUnitInRaid(SettlmentArmy settlmentArmy, Unit unit, int marchingTime)
        {
            settlmentArmy.army.RemoveUnit(unit);
            attackingArmy.AddUnit(unit, settlmentArmy.settlment, marchingTime);
            RefreshArmiesUi();
        }

        private void ReturnUnitToSettlment(AttackingUnit unit)
        {
            AttackingUnit attackingUnit = (AttackingUnit)unit;
            SettlmentArmy settlmentArmy = availableArmies.Find((a) => a.settlment == attackingUnit.OriginalSettlment);
            attackingArmy.RemoveUnit(attackingUnit);
            settlmentArmy.army.AddUnit(new Unit(attackingUnit));
            RefreshArmiesUi();
        }

        private void SendArmy()
        {
            if (attackingArmy.UnitsCount != 0)
            {
                //Sendable
                foreach (var unit in attackingArmy.GetUnits())
                {
                    if (unit.OriginalSettlment != null)
                    {
                        var army = level.Player.GetSettlmentArmy(unit.OriginalSettlment);
                        army.RemoveUnit(unit.EnclosedUnit);

                        level.Player.AddMarchingUnit(unit.EnclosedUnit, attackedSettlment, unit.MarchingTime);
                    }
                }

                CreateAttackingArmy();
                RefreshArmiesUi();
            }
        }

        private void CreateAttackingArmy()
        {
            attackingArmy = new AttackingArmy();
            var settlmentArmy = level.Player.GetSettlmentArmy(attackedSettlment);
            if (settlmentArmy != null)
            {
                foreach (var unit in settlmentArmy.GetUnits())
                {
                    attackingArmy.AddUnit(unit, null, 0);
                }
            }

            foreach (var marchingUnit in level.Player.GetMarchingUnits(attackedSettlment))
            {
                attackingArmy.AddUnit(marchingUnit.unit, null, marchingUnit.daysLeft);
            }
        }
    }
}
