using UnityEngine;
using System;


namespace Domination.Battle.Logic
{
    public class Structure : IMapUnit
    {
        public Action<Structure> OnDestroyed;

        public readonly bool isGate;

        private int health;

        
        public MapUnitType Type => MapUnitType.Structure;

        public Vector2Int Position { get; private set; }


        public Structure(bool isGate, Vector2Int position, int health)
        {
            this.isGate = isGate;
            this.health = health;
            Position = position;
        }

        public void ReceiveDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                OnDestroyed?.Invoke(this);
            }
        }
    }
}
