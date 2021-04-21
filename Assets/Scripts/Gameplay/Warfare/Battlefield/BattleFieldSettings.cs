using UnityEngine;


namespace Domination.Battle.Settings
{
    public class BattleFieldSettings : ScriptableObject
    {
        private static readonly ResourceAsset<BattleFieldSettings> asset = new ResourceAsset<BattleFieldSettings>("BattleFieldSettings");

        [Header("Map generation")]
        [SerializeField] private int attackersMinDistanceFromWall = default;
        [SerializeField] private float tagetBattleFieldSizeRatio = default;
        [Header("Units")]
        [SerializeField] private float minSpeedMultiplier = default;
        [SerializeField] private float maxSpeedMultiplier = default;
        [SerializeField] private float maxMovementDelay = default;
        [SerializeField] private float maxAttackDelay = default;


        public static int AttackersMinDistanceFromWall => asset.Value.attackersMinDistanceFromWall;
        public static float TagetBattleFieldSizeRatio => asset.Value.tagetBattleFieldSizeRatio;

        public static float GetRandomSpeedMultiplier() => Random.Range(asset.Value.minSpeedMultiplier, asset.Value.maxSpeedMultiplier);
        public static float GetRandomMovementDelay() => Random.Range(0.0f, asset.Value.maxMovementDelay);
        public static float GetRandomAttackDelay() => Random.Range(0, asset.Value.maxAttackDelay);
    }
}
