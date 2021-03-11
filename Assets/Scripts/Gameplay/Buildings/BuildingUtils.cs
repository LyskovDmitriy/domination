namespace Domination
{
    public static class BuildingUtils
    {
        public static int GetUpgradePrice(BuildingType buildingType, int buildingCurrentLevel) =>
            BuildingsSettingsContainer.GetBuildingSettings(buildingType).GetLevelPrice(buildingCurrentLevel + 1);

        public static int GetConstructionPrice(BuildingType buildingType) =>
            BuildingsSettingsContainer.GetBuildingSettings(buildingType).GetLevelPrice(0);
    }
}
