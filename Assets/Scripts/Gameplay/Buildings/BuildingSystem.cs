using System;
using System.Collections.Generic;
using System.Linq;

public enum UpdatePossibility
{
    AlreadyHighestLevel,
    NotEnoughMoney,
    Possible
}

public static class BuildingSystem
{
    private static Level level;


    public static void Init(Level currentLevel)
    {
        level = currentLevel;
    }


    public static UpdatePossibility CanUpdateBuilding(int characterId, SettlmentType settlmentType, BuildingType buildingType, int buildingLevel)
    {
        int maxPossibleBuildingLevel = SettlmentsSettings.GetMaxBuildingLevel(settlmentType, buildingType);
        if (buildingLevel >= maxPossibleBuildingLevel)
        {
            return UpdatePossibility.AlreadyHighestLevel;
        }

        int upgradePrice = GetUpgradePrice(buildingType, buildingLevel);
        return (upgradePrice <= level.GetCharacterById(characterId).Coins) ? UpdatePossibility.Possible : UpdatePossibility.NotEnoughMoney;
    }


    public static bool CanBuild(int settlmentId, BuildingType buildingType)
    {
        return GetCharacterWithSettlment(settlmentId).Coins >= GetConstructionPrice(buildingType);
    }


    public static void UpgradeBuilding(int settlmentId, BuildingType buildingType)
    {
        //Can add check but will leave it out for now
        GetCharacterWithSettlment(settlmentId).UpgradeBuilding(settlmentId, buildingType);
    }


    public static void DestroyBuilding(int settlmentId, BuildingType buildingType)
    {
        //Can add check but will leave it out for now
        GetCharacterWithSettlment(settlmentId).DestroyBuilding(settlmentId, buildingType);
    }


    public static int GetUpgradePrice(BuildingType buildingType, int buildingCurrentLevel) => BuildingsSettingsContainer.GetBuildingSettings(buildingType).GetLevelPrice(buildingCurrentLevel + 1);

    public static int GetConstructionPrice(BuildingType buildingType) => BuildingsSettingsContainer.GetBuildingSettings(buildingType).GetLevelPrice(0);

    public static BuildingType[] GetAvailableBuildings(int settlmentId)
    {
        Settlment settlment = GetSettlment(settlmentId);
        var buildings = settlment.GetBuildings().Select((building) => building.type).ToList();
        var allAvailableBuildings = SettlmentsSettings.GetAvailableBuildingsForSettlment(settlment.Type).Select((building) => building.type).ToList();

        return allAvailableBuildings.Except(buildings).ToArray();
    }

    public static void Build(int settlmentId, BuildingType buildingType)
    {
        GetCharacterWithSettlment(settlmentId).Build(settlmentId, buildingType);
    }

    private static Character GetCharacterWithSettlment(int settlmentId) => Array.Find(level.Characters, character => character.HasSettlment(settlmentId));

    private static Settlment GetSettlment(int settlmentId)
    {
        return GetCharacterWithSettlment(settlmentId).GetSettlmentById(settlmentId);
    }
}
