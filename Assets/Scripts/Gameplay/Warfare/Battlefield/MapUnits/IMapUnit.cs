using UnityEngine;


namespace Domination.Battle.Logic
{
    public interface IMapUnit
    {
        MapUnitType Type { get; }
        public Vector2Int Position { get; }
    }
}
