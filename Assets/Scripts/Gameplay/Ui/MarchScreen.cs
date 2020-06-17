using Domination.Warfare;
using System.Collections.Generic;
using UnityEngine;
using Utils.Ui;


namespace Domination.Ui
{
    public class MarchScreen : UiScreen
    {
        [SerializeField] private SettlmentArmyUi settlmentArmyPrefab = default;
        [SerializeField] private Transform settlmentArmiesRoot = default;

        private List<SettlmentArmyUi> armies = new List<SettlmentArmyUi>();


        public override ScreenType Type => ScreenType.MarchScreen;


        public void Show(Character player, Settlment targetSettlment)
        {
            base.Show();

            foreach (var army in armies)
            {
                Destroy(army.gameObject);
            }
            armies.Clear();

            foreach (var settlment in player.Settlments)
            {
                List<Unit> units = settlment.Army.GetUnits();

                SettlmentArmyUi settlmentArmyUi = Instantiate(settlmentArmyPrefab, settlmentArmiesRoot);
                settlmentArmyUi.Init(units);
                settlmentArmyUi.gameObject.SetActive(true);
                armies.Add(settlmentArmyUi);
            }
        }
    }
}
