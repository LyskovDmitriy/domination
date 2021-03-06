using Domination.Data;
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
                Build(buildingInfo.type, buildingInfo.defaultLevel);
            }
        }

        public Castle(SettlmentData data) : base(data) { }
    }
}
