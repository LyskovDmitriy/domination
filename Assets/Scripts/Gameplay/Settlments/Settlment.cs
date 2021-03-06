using Domination.Data;
using Domination.Warfare;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Domination.LevelLogic
{
    public abstract class Settlment
    {
        public class Building
        {
            public BuildingType type;
            public int level;


            public BuildingData GetData() => new BuildingData
            {
                type = type,
                level = level
            };
        }

        private static uint NextId = 1;

        private List<Building> buildings = new List<Building>();

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

        public string Title => $"{Type}_{Id}";

        public uint Id { get; private set; }

        public Vector2Int Position { get; private set; }

        public Character Lord { get; set; }


        public Settlment(Vector2Int position)
        {
            Position = position;
            Id = NextId;
            NextId++;
        }

        public Settlment(SettlmentData data)
        {
            Id = data.id;

            Position = Position;

            foreach (var building in data.buildings)
            {
                Build(building.type, building.level);
            }
        }


        public SettlmentData GetData() => new SettlmentData
        {
            id = Id,
            type = Type,
          
            position = Position,
            ownerId = (Lord == null) ? 0 : Lord.Id,
            
            buildings = buildings.Select(b => b.GetData()).ToArray()
        };

        public void DestroyBuilding(BuildingType buildingType)
        {
            Building destroyedBuilding = GetBuilding(buildingType);
            buildings.Remove(destroyedBuilding);
        }

        public void UpgradeBuilding(BuildingType buildingType) => GetBuilding(buildingType).level++;

        public List<Building> GetBuildings() => new List<Building>(buildings);

        public bool HasBuilding(BuildingType type) => (GetBuilding(type) != null);

        public Building GetBuilding(BuildingType type) => buildings.Find((b) => b.type == type);

        public void Recruit(Unit unit) => Lord.Recruit(unit, Id);

        public Army GetArmy() => Lord.GetSettlmentArmy(this);


        public void Build(BuildingType buildingType)
        {
            if (buildings.Exists((building) => building.type == buildingType))
            {
                Debug.LogError($"Trying to build already build {buildingType}");
                return;
            }
            buildings.Add(new Building { type = buildingType });
        }

        protected void Build(BuildingType type, int level)
        {
            Build(type);

            for (int i = 1; i <= level; i++)
            {
                UpgradeBuilding(type);
            }
        }
    }
}
