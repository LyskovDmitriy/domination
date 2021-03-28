using Domination.Warfare;
using System;
using UnityEngine;


namespace Domination
{
    [Serializable]
    public class UnitsGroup
    {
        [SerializeField] private int unitsCount = default;
        [SerializeField] private WeaponType weaponType = default;
        [SerializeField] private int weaponLevel = default;

        public int UnitsCount => unitsCount;
        public WeaponType WeaponType => weaponType;
        public int WeaponLevel => weaponLevel;
    }


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
        [SerializeField] private UnitsGroup[] neutralVillageArmy = default;


        public static BuildingSettingsBase GetBuildingInfo(BuildingType type) => 
            Array.Find(asset.Instance.buildings, (building) => building.Type == type);

        public static T GetBuildingInfo<T>() where T : BuildingSettingsBase =>
            Array.Find(asset.Instance.buildings, (building) => building is T) as T;

        public static BuildingInfo[] AvailableBuildingsInCity => asset.Instance.availableBuildingsInCastle;
        public static BuildingInfo[] AvailableBuildingsInVillage => asset.Instance.availableBuildingsInVillage;

        public static UnitsGroup[] NeutralVillageArmy => asset.Instance.neutralVillageArmy;

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

        public static BuildingInfo[] GetAvailableBuildingsForSettlment(SettlmentType settlmentType) => (settlmentType == SettlmentType.Castle) ?
                                                                                    asset.Instance.availableBuildingsInCastle : asset.Instance.availableBuildingsInVillage;

        public static int GetMaxBuildingLevel(SettlmentType settlment, BuildingType building)
        {
            BuildingInfo[] buildingsInSettlment = GetAvailableBuildingsForSettlment(settlment);
            return Array.Find(buildingsInSettlment, (buildingInfo) => buildingInfo.type == building).maxLevel;
        }
    } 
}
