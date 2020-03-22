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
    [SerializeField] private BuildingInfo[] availableBuildingsInCity = default;
    [SerializeField] private int villageMaxBuildingsCount = default;
    [SerializeField] private BuildingInfo[] availableBuildingsInVillage = default;
    [SerializeField] private BuildingSettingsBase[] buildings = default;


    public static BuildingSettingsBase GetBuildingInfo(BuildingType type) => Array.Find(asset.Instance.buildings, (building) => building.Type == type);

    public static BuildingInfo[] AvailableBuildingsInCity => asset.Instance.availableBuildingsInCity;

    public static int VillageMaxBuildingsCount => asset.Instance.villageMaxBuildingsCount;
    public static int CastleMaxBuildingsCount => asset.Instance.castleMaxBuildingsCount;
}
