using Domination.Battle.Logic.Ai;
using Domination.Warfare;
using UnityEngine;
using System;


namespace Domination.Battle.Logic
{
    public class Warrior : IMapUnit
    {
        public readonly Unit Unit;
        public readonly bool IsAttacker;

        public readonly ITurnPlanner planner;


        public Vector2Int Position { get; private set; }

        public MapUnitType Type => MapUnitType.Warrior;


        public Warrior(Unit unit, bool isAttacker, Vector2Int position)
        {
            Unit = unit;
            IsAttacker = isAttacker;
            Position = position;

            planner = new MeleeUnitPlanner(this);
        }

        public void PlanTurn(IMapUnit[,] planningMap, BattlePathfiner.PathfinidingResult pathfinidingResult)
        {
            planner.PlanTurn(planningMap, pathfinidingResult);
        }

        public void ExecuteTurn() => planner.ExecuteTurn();

        public void Move(Vector2Int position)
        {
            var previousPosition = Position;
            Position = position;
        }
    }
}
