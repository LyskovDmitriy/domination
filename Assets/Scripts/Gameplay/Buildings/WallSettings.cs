using System;
using UnityEngine;


[CreateAssetMenu]
public class WallSettings : BuildingSettings<WallSettings.LevelInfo>
{
    [Serializable]
    public class LevelInfo : DefaultLevelInfo
    {
        [SerializeField] private int wallUnitHealth = default;
        [SerializeField] private int gateUnitHealth = default;
        [SerializeField] private int gateHeight = default;
        [SerializeField] private int minWallHeight = default;
        [SerializeField] private int wallThickness = default;

        public int WallUnitHealth => wallUnitHealth;
        public int GateUnitHealth => gateUnitHealth;
        public int GateHeight => gateHeight;
        public int MinWallHeight => minWallHeight;
        public int WallThickness => wallThickness;
    }


    public override BuildingType Type => BuildingType.Wall;
}
