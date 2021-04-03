using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Domination.Battle.Settings;


namespace Domination.Battle.Logic
{
    public class ArmyCommander
    {
        public readonly bool IsAttacker;
        
        private readonly BattleController battleController;

        private IMapUnit[,] currentPlanningMap;

        private HashSet<Warrior> warriors = new HashSet<Warrior>();


        public ArmyCommander(BattleController battleController, bool isAttacker)
        {
            this.battleController = battleController;
            IsAttacker = isAttacker;
        }

        public void AddWarrior(Warrior warrior)
        {
            warriors.Add(warrior);
        }

        public void PlanTurn(IMapUnit[,] planningMap)
        {
            currentPlanningMap = planningMap;

            var orderedUnits = warriors.OrderBy(GetOrder).ToList();

            while (orderedUnits.Count() > 0)
            {
                var warriorsThatCanMove = orderedUnits.Where(CanMove);

                Warrior warrior;
                if (warriorsThatCanMove.Count() > 0)
                {
                    warrior = warriorsThatCanMove.First();
                }
                else
                {
                    warrior = orderedUnits.First();
                }


                orderedUnits.Remove(warrior);
            }
            //Plan turn for each unit
            //So that view could execute plan
            currentPlanningMap = null;
        }

        public void ExecuteTurn()
        {
            //
        }

        private bool CanMove(Warrior warrior) =>
            battleController.IsTileEmpty(currentPlanningMap, warrior.Position + Vector2Int.up) ||
            battleController.IsTileEmpty(currentPlanningMap, warrior.Position + Vector2Int.down) ||
            battleController.IsTileEmpty(currentPlanningMap, warrior.Position + Vector2Int.right) ||
            battleController.IsTileEmpty(currentPlanningMap, warrior.Position + Vector2Int.left);

        //if is attacker, units further from 0 on x axis are prioretized
        //if is defender, units closer to 0 on x axis are prioretized
        private int GetOrder(Warrior warrior) => warrior.Position.x * (IsAttacker ? 1 : -1);

        private int GetPassingCost(IMapUnit mapUnit) => MapUnitsPassingCost.GetCost(mapUnit, IsAttacker);
    }
}
