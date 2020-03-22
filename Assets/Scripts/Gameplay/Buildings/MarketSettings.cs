using System;
using UnityEngine;


[CreateAssetMenu]
public class MarketSettings : BuildingSettings<MarketSettings.LevelInfo>
{
    [Serializable]
    public class LevelInfo : DefaultLevelInfo
    {
        [SerializeField] private int turnIncome = default;


        public int TurnIncome => turnIncome;
    }


    public override BuildingType Type => BuildingType.Market;
}
