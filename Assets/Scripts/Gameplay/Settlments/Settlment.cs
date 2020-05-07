using System.Collections.Generic;
using UnityEngine;


public abstract class Settlment : MonoBehaviour
{
    public class Building
    {
        public BuildingType type;
        public int level;
    }

    private static int NextId = 0;

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

    public int Id { get; private set; }


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


    public Building GetBuilding(BuildingType type) => buildings.Find((b) => b.type == type);


    public void Build(BuildingType buildingType)
    {
        if (buildings.Exists((building) => building.type == buildingType))
        {
            Debug.LogError($"Trying to build already build {buildingType}");
            return;
        }
        buildings.Add(new Building { type = buildingType });
    }
}
