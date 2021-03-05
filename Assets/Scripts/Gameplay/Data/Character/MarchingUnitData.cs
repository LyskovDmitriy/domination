using System;


namespace Domination.Data
{
    [Serializable]
    public class MarchingUnitData
    {
        public UnitData unitData;
        public uint targetSettlment;
        public int daysLeft;
    }
}
