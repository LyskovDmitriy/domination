using UnityEngine;


public class DefaultLevelInfo
{
    [SerializeField] private int price;
}


public abstract class BuildingSettings<LevelInfo> : BuildingSettingsBase where LevelInfo : DefaultLevelInfo
{
    [SerializeField] private LevelInfo[] levels = default;


    public LevelInfo[] Levels => levels;
}
