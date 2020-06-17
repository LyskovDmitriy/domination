using Domination.Warfare;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Domination
{
    public abstract class Settlment : MonoBehaviour
    {
        public class Building
        {
            public BuildingType type;
            public int level;
        }

        private static int NextId = 0;

        public event Action OnUnitsChange;

        private List<Building> buildings = new List<Building>();


        public Army Army { get; private set; } = new Army();

        public int Income
        {
            get
            {
                Building market = buildings.Find((building) => building.type == BuildingType.Market);

                if (market == null)
                {
                    return 0;
                }

                MarketSettings settings = SettlmentsSettings.GetBuildingInfo(BuildingType.Market) as MarketSettings;
                return settings.Levels[market.level].TurnIncome;
            }
        }

        public abstract SettlmentType Type { get; }

        public int Id { get; private set; }

        public Character Lord { get; set; }

        public Tile Tile { get; set; }


        protected virtual void Awake()
        {
            Id = NextId;
            NextId++;
        }


        public void DestroyBuilding(BuildingType buildingType)
        {
            Building destroyedBuilding = GetBuilding(buildingType);
            buildings.Remove(destroyedBuilding);
        }


        public void UpgradeBuilding(BuildingType buildingType)
        {
            GetBuilding(buildingType).level++;
        }


        public List<Building> GetBuildings() => new List<Building>(buildings);

        public bool HasBuilding(BuildingType type) => (GetBuilding(type) != null);

        public Building GetBuilding(BuildingType type) => buildings.Find((b) => b.type == type);

        public int GetUnitsCount(WeaponType weaponType) => Army.GetUnitsCount(weaponType);


        public void Build(BuildingType buildingType)
        {
            if (buildings.Exists((building) => building.type == buildingType))
            {
                Debug.LogError($"Trying to build already build {buildingType}");
                return;
            }
            buildings.Add(new Building { type = buildingType });
        }


        public void Recruit(Unit unit)
        {
            Army.AddUnit(unit);
            OnUnitsChange?.Invoke();
        }
    }
}
