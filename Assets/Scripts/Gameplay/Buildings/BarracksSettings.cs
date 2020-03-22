using System;
using UnityEngine;


[CreateAssetMenu]
public class BarracksSettings : BuildingSettings<BarracksSettings.LevelInfo>
{
    [Serializable]
    public class LevelInfo : DefaultLevelInfo
    {
        [SerializeField] private float unitsCreationDurationModifier = default;
    }


    public override BuildingType Type => BuildingType.Barracks;
}
