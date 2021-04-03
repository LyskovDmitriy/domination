using Domination.Battle.Logic;
using UnityEngine;


namespace Domination.Battle.Settings
{
    [CreateAssetMenu]
    public class MapUnitsPassingCost : ScriptableObject
    {
        private static readonly ResourceAsset<MapUnitsPassingCost> asset = new ResourceAsset<MapUnitsPassingCost>("MapUnitsPassingCost");

        [SerializeField] private int gatePassingCost = default;
        [SerializeField] private int wallPassingCost = default;
        [SerializeField] private int allyPassingCost = default;
        [SerializeField] private int enemyPassingCost = default;

        public static int GetCost(IMapUnit mapUnit, bool isUnitAttacker)
        {
            if (mapUnit == null)
            {
                return 1;
            }

            if (mapUnit is Structure structure)
            {
                return structure.isGate ? asset.Value.gatePassingCost: asset.Value.wallPassingCost;
            }

            var warrior = (Warrior)mapUnit;
            return (warrior.IsAttacker == isUnitAttacker) ? asset.Value.allyPassingCost : asset.Value.enemyPassingCost;
        }
    }
}
