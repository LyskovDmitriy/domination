using Domination.LevelLogic;
using System.Linq;


public enum UpdatePossibility
{
    AlreadyHighestLevel,
    NotEnoughMoney,
    Possible
}


namespace Domination
{
    public static class BuildingSystem
    {
        private static Level level;


        public static void Init(Level currentLevel) => level = currentLevel;

        public static UpdatePossibility CanUpdateBuilding(uint settlmentId, SettlmentType settlmentType, BuildingType buildingType, int buildingLevel)
        {
            int maxPossibleBuildingLevel = SettlmentsSettings.GetMaxBuildingLevel(settlmentType, buildingType);
            if (buildingLevel >= maxPossibleBuildingLevel)
            {
                return UpdatePossibility.AlreadyHighestLevel;
            }

            int upgradePrice = GetUpgradePrice(buildingType, buildingLevel);
            return GetSettlmentLord(settlmentId).HasCoins(upgradePrice) ? 
                UpdatePossibility.Possible : UpdatePossibility.NotEnoughMoney;
        }

        public static bool CanBuild(uint settlmentId, BuildingType buildingType) =>
            GetSettlmentLord(settlmentId).HasCoins(GetConstructionPrice(buildingType));

        public static void UpgradeBuilding(uint settlmentId, BuildingType buildingType) =>
            GetSettlmentLord(settlmentId).UpgradeBuilding(settlmentId, buildingType);

        public static void DestroyBuilding(uint settlmentId, BuildingType buildingType) => 
            GetSettlmentLord(settlmentId).DestroyBuilding(settlmentId, buildingType);

        public static int GetUpgradePrice(BuildingType buildingType, int buildingCurrentLevel) => 
            BuildingsSettingsContainer.GetBuildingSettings(buildingType).GetLevelPrice(buildingCurrentLevel + 1);

        public static int GetConstructionPrice(BuildingType buildingType) => 
            BuildingsSettingsContainer.GetBuildingSettings(buildingType).GetLevelPrice(0);

        public static BuildingType[] GetAvailableBuildings(uint settlmentId)
        {
            Settlment settlment = level.GetSettlment(settlmentId);
            var buildings = settlment.GetBuildings().Select((building) => building.type).ToList();
            var allAvailableBuildings = SettlmentsSettings.GetAvailableBuildingsForSettlment(settlment.Type).Select((building) => building.type).ToList();

            return allAvailableBuildings.Except(buildings).ToArray();
        }

        public static void Build(uint settlmentId, BuildingType buildingType) => 
            GetSettlmentLord(settlmentId).Build(settlmentId, buildingType);

        private static Character GetSettlmentLord(uint settlmentId) => 
            level.GetSettlment(settlmentId).Lord;
    }
}
