namespace Domination
{
    public static class BuildingUtils
    {
        public static int GetUpgradePrice(BuildingType buildingType, int buildingCurrentLevel) =>
            SettlmentsSettings.GetBuildingInfo(buildingType).GetLevelPrice(buildingCurrentLevel + 1);

        public static int GetConstructionPrice(BuildingType buildingType) =>
            SettlmentsSettings.GetBuildingInfo(buildingType).GetLevelPrice(0);
    }
}
