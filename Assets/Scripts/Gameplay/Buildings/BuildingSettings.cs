using UnityEngine;


public class DefaultLevelInfo
{
    [SerializeField] private int price;


    public int Price => price;
}


public abstract class BuildingSettings<LevelInfo> : BuildingSettingsBase where LevelInfo : DefaultLevelInfo
{
    [SerializeField] private LevelInfo[] levels = default;


    public LevelInfo[] Levels => levels;


    public override int GetLevelPrice(int level) => levels[level].Price;
}
