using UnityEngine;


namespace Domination.Battle.Settings
{
    public class BattleFieldSettings : ScriptableObject
    {
        private static readonly ResourceAsset<BattleFieldSettings> asset = new ResourceAsset<BattleFieldSettings>("BattleFieldSettings");

        [SerializeField] private int attackersMinDistanceFromWall = default;
        [SerializeField] private float tagetBattleFieldSizeRatio = default;


        public static int AttackersMinDistanceFromWall => asset.Value.attackersMinDistanceFromWall;
        public static float TagetBattleFieldSizeRatio => asset.Value.tagetBattleFieldSizeRatio;
    }
}
