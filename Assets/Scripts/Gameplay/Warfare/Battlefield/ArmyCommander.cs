using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Domination.Battle.Settings;


namespace Domination.Battle.Logic
{
    public class ArmyCommander
    {
        public readonly bool IsAttacker;
        public readonly HashSet<Warrior> Warriors = new HashSet<Warrior>();
        
        private readonly BattleController battleController;


        public ArmyCommander(BattleController battleController, bool isAttacker)
        {
            this.battleController = battleController;
            IsAttacker = isAttacker;
        }

        public void AddWarrior(Warrior warrior)
        {
            Warriors.Add(warrior);
        }

        //Plan turn for each unit
        //So that view could execute plan
        public void PlanTurn(IMapUnit[,] planningMap)
        {
            var orderedUnits = Warriors.OrderBy(GetOrder).ToList();

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

                var pathfindingData = BattlePathfiner.GetPathfindingData(
                    warrior.Position, 
                    planningMap, 
                    GetPassingCost,
                    mapUnit => mapUnit.IsAttacker != IsAttacker);

                warrior.PlanTurn(planningMap, pathfindingData);

                orderedUnits.Remove(warrior);
            }

            bool CanMove(Warrior warrior) =>
                battleController.IsTileEmpty(planningMap, warrior.Position + Vector2Int.up) ||
                battleController.IsTileEmpty(planningMap, warrior.Position + Vector2Int.down) ||
                battleController.IsTileEmpty(planningMap, warrior.Position + Vector2Int.right) ||
                battleController.IsTileEmpty(planningMap, warrior.Position + Vector2Int.left);
        }

        public void ExecuteTurn()
        {
            foreach (var warrior in Warriors)
            {
                warrior.ExecuteTurn();
            }
        }

        //if is attacker, units further from 0 on x axis are prioretized
        //if is defender, units closer to 0 on x axis are prioretized
        private int GetOrder(Warrior warrior) => warrior.Position.x * (IsAttacker ? 1 : -1);

        private int GetPassingCost(IMapUnit mapUnit) => MapUnitsPassingCost.GetCost(mapUnit, IsAttacker);
    }
}
