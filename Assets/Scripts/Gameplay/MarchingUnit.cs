using Domination.LevelLogic;
using Domination.Warfare;
using Domination.Data;


namespace Domination
{
    public class MarchingUnit
    {
        public Settlment targetSettlment;
        public Unit unit;
        public int daysLeft;


        public MarchingUnitData GetData() => new MarchingUnitData
        {
            targetSettlment = targetSettlment.Id,
            unitData = unit.GetData(),
            daysLeft = daysLeft,
        };

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is MarchingUnit marchingUnit))
            {
                return false;
            }

            return (targetSettlment == marchingUnit.targetSettlment) &&
                unit.Equals(marchingUnit.unit) &&
                (daysLeft == marchingUnit.daysLeft);
        }
    }
}