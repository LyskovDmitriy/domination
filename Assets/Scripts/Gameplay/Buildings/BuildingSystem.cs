using Domination.LevelLogic;
using System;
using System.Linq;


public enum UpdatePossibility
{
    AlreadyHighestLevel,
    NotEnoughMoney,
    Possible
}


namespace Domination
{
    public class BuildingSystem
    {
        private Func<uint, Settlment> getSettlmentAction;


        public BuildingSystem(Func<uint, Settlment> getSettlmentAction) => this.getSettlmentAction = getSettlmentAction;

        public UpdatePossibility CanUpdateBuilding(uint settlmentId, SettlmentType settlmentType, BuildingType buildingType, int buildingLevel)
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

        public bool CanBuild(uint settlmentId, BuildingType buildingType) =>
            GetSettlmentLord(settlmentId).HasCoins(GetConstructionPrice(buildingType));

        public void UpgradeBuilding(uint settlmentId, BuildingType buildingType) =>
            GetSettlmentLord(settlmentId).UpgradeBuilding(settlmentId, buildingType);

        public void DestroyBuilding(uint settlmentId, BuildingType buildingType) => 
            GetSettlmentLord(settlmentId).DestroyBuilding(settlmentId, buildingType);

        public int GetUpgradePrice(BuildingType buildingType, int buildingCurrentLevel) => 
            BuildingsSettingsContainer.GetBuildingSettings(buildingType).GetLevelPrice(buildingCurrentLevel + 1);

        public int GetConstructionPrice(BuildingType buildingType) => 
            BuildingsSettingsContainer.GetBuildingSettings(buildingType).GetLevelPrice(0);

        public BuildingType[] GetAvailableBuildings(uint settlmentId)
        {
            Settlment settlment = getSettlmentAction(settlmentId);
            var buildings = settlment.GetBuildings().Select((building) => building.type).ToList();
            var allAvailableBuildings = SettlmentsSettings.GetAvailableBuildingsForSettlment(settlment.Type).Select((building) => building.type).ToList();

            return allAvailableBuildings.Except(buildings).ToArray();
        }

        public void Build(uint settlmentId, BuildingType buildingType) => 
            GetSettlmentLord(settlmentId).Build(settlmentId, buildingType);

        private Character GetSettlmentLord(uint settlmentId) => getSettlmentAction(settlmentId).Lord;
    }
}
