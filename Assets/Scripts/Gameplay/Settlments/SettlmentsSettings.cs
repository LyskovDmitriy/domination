using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class SettlmentsSettings : ScriptableObject
{
    [Serializable]
    public class BuildingInfo
    {
        public BuildingType type;
        public int defaultLevel = -1;
        public int maxLevel;
    }

    private static readonly ResourceAsset<SettlmentsSettings> asset = new ResourceAsset<SettlmentsSettings>("SettlmentsSettings");

    [SerializeField] private int castleMaxBuildingsCount = default;
    [SerializeField] private BuildingInfo[] availableBuildingsInCastle = default;
    [SerializeField] private int villageMaxBuildingsCount = default;
    [SerializeField] private BuildingInfo[] availableBuildingsInVillage = default;
    [SerializeField] private BuildingSettingsBase[] buildings = default;


    public static BuildingSettingsBase GetBuildingInfo(BuildingType type) => Array.Find(asset.Instance.buildings, (building) => building.Type == type);

    public static BuildingInfo[] AvailableBuildingsInCity => asset.Instance.availableBuildingsInCastle;

    public static int GetMaxBuildingsCount(SettlmentType settlmentType)
    {
        switch (settlmentType)
        {
            case SettlmentType.Village:
                return asset.Instance.villageMaxBuildingsCount;
            case SettlmentType.Castle:
                return asset.Instance.castleMaxBuildingsCount;
        }

        return 0;
    }

    public static int GetMaxBuildingLevel(SettlmentType settlment, BuildingType building)
    {
        BuildingInfo[] buildingsInSettlment = settlment == SettlmentType.Castle ? asset.Instance.availableBuildingsInCastle : asset.Instance.availableBuildingsInVillage;
        return Array.Find(buildingsInSettlment, (buildingInfo) => buildingInfo.type == building).maxLevel;
    }
}
