using Domination.Data;
using UnityEngine;


namespace Domination.LevelLogic
{
    public class Castle : Settlment
    {
        public override SettlmentType Type => SettlmentType.Castle;


        public Castle(Vector2Int position) : base(position)
        {
            //TODO: Setup from the outside
            foreach (var buildingInfo in SettlmentsSettings.AvailableBuildingsInCity)
            {
                Build(buildingInfo.type, buildingInfo.defaultLevel);
            }
        }

        public Castle(SettlmentData data) : base(data) { }
    }
}
