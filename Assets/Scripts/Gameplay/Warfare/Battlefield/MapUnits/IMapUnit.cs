using UnityEngine;


namespace Domination.Battle.Logic
{
    public interface IMapUnit
    {
        MapUnitType Type { get; }
        Vector2Int Position { get; }

        void ReceiveDamage(int damage);
    }
}
