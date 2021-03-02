using Domination.LevelLogic;
using UnityEngine;


namespace Domination
{
    public class Castle : Settlment
    {
        public override SettlmentType Type => SettlmentType.Castle;


        public Castle(Vector2Int position) : base(position)
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
}
