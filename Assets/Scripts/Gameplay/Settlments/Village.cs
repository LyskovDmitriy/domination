using Domination.Data;
using UnityEngine;


namespace Domination.LevelLogic
{
    public class Village : Settlment
    {
        public override SettlmentType Type => SettlmentType.Village;


        public Village(Vector2Int position) : base(position) 
        {
            //TODO: Setup from outside
            foreach (var buildingInfo in SettlmentsSettings.AvailableBuildingsInVillage)
            {
                Build(buildingInfo.type, buildingInfo.defaultLevel);
            }
        }

        public Village(SettlmentData data) : base(data) { }
    }
}
