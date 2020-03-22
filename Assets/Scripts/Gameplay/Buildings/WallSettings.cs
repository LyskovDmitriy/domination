using System;
using UnityEngine;


[CreateAssetMenu]
public class WallSettings : BuildingSettings<WallSettings.LevelInfo>
{
    [Serializable]
    public class LevelInfo : DefaultLevelInfo
    {
        [SerializeField] private int health = default;
    }


    public override BuildingType Type => BuildingType.Wall;
}
