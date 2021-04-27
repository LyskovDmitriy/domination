using Domination.Battle.Logic.Ai;
using Domination.Warfare;
using UnityEngine;
using System;


namespace Domination.Battle.Logic
{
    public class Warrior : IMapUnit
    {
        public Action<Warrior> OnDied;

        public readonly Unit Unit;
        public readonly bool IsAttacker;

        public readonly ITurnPlanner planner;

        private int health;
        private bool isDead;


        public Vector2Int Position { get; private set; }

        public MapUnitType Type => MapUnitType.Warrior;


        public Warrior(Unit unit, bool isAttacker, Vector2Int position)
        {
            Unit = unit;
            IsAttacker = isAttacker;
            Position = position;

            health = Unit.Health;

            planner = new MeleeUnitPlanner(this);
        }

        public void PlanTurn(IMapUnit[,] planningMap, BattlePathfiner.PathfinidingResult pathfinidingResult)
        {
            planner.PlanTurn(planningMap, pathfinidingResult);
        }

        public void ExecuteTurn() => planner.ExecuteTurn();

        public void Move(Vector2Int position)
        {
            Position = position;
        }

        public void ReceiveDamage(int damage)
        {
            health -= damage;

            if ((health <= 0) && !isDead)
            {
                isDead = true;
                OnDied?.Invoke(this);
            }
        }
    }
}
