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


    public int Income
    {
        get
        {
            Building market = Buildings.Find((building) => building.type == BuildingType.Market);

            if (market == null)
            {
                return 0;
            }

            MarketSettings settings = SettlmentsSettings.GetBuildingInfo(BuildingType.Market) as MarketSettings;
            return settings.Levels[market.level].TurnIncome;
        }
    }


    public List<Building> Buildings { get; private set; } = new List<Building>();

    public abstract SettlmentType Type { get; }

    public int Id { get; private set; }


    protected virtual void Awake()
    {
        Id = NextId;
        NextId++;
    }


    public void DestroyBuilding(BuildingType buildingType)
    {
        Building destroyedBuilding = Buildings.Find((building) => building.type == buildingType);
        Buildings.Remove(destroyedBuilding);
    }


    protected void Build(BuildingType buildingType)
    {
        if (Buildings.Exists((building) => building.type == buildingType))
        {
            Debug.LogError($"Trying to build already build {buildingType}");
            return;
        }
        Buildings.Add(new Building { type = buildingType });
    }


    protected void UpgradeBuilding(BuildingType buildingType)
    {
        Building building = Buildings.Find((b) => b.type == buildingType);
        building.level++;
    }
}
