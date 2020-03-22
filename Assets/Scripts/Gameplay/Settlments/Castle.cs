using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Castle : Settlment
{
    public override int MaxBuildingsCount => SettlmentsSettings.CastleMaxBuildingsCount;


    private void Awake()
    {
        foreach (var buildingInfo in SettlmentsSettings.AvailableBuildingsInCity)
        {
            for (int i = 0; i < buildingInfo.defaultLevel; i++)
            {
                if (i == 0)
                {
                    Build(buildingInfo.type);
                }
                else
                {
                    UpgradeBuilding(buildingInfo.type);
                }
            }
        }
    }
}
