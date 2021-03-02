using Domination.LevelLogic;


namespace Domination.Warfare
{
    public class AttackingUnit : Unit
    {
        public Settlment OriginalSettlment { get; private set; }
        public Unit EnclosedUnit { get; private set; }
        public int MarchingTime { get; private set; }


        public AttackingUnit(Unit unit, Settlment settlment, int marchingTime) : base(unit)
        {
            EnclosedUnit = unit;
            OriginalSettlment = settlment;
            MarchingTime = marchingTime;
        }
    }
}
