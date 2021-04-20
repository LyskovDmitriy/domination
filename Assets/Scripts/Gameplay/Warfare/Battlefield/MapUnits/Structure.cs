using UnityEngine;


namespace Domination.Battle.Logic
{
    public class Structure : IMapUnit
    {
        public readonly bool isGate;

        
        public MapUnitType Type => MapUnitType.Structure;

        public Vector2Int Position { get; private set; }


        public Structure(bool isGate, Vector2Int position)
        {
            this.isGate = isGate;
            Position = position;
        }
    }
}
