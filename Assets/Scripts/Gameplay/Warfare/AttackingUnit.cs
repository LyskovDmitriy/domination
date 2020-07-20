namespace Domination.Warfare
{
    public class AttackingUnit : Unit
    {
        public Settlment originalSettlment;


        public AttackingUnit(Unit unit, Settlment settlment) : base(unit)
        {
            originalSettlment = settlment;
        }
    }
}
