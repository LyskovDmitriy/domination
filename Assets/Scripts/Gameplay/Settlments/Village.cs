using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Village : Settlment
{
    public override int MaxBuildingsCount => SettlmentsSettings.VillageMaxBuildingsCount;
}
