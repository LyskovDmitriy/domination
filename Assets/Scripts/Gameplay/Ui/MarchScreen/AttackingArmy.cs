using Domination.LevelLogic;
using Domination.Warfare;
using System.Collections.Generic;


namespace Domination.Ui.Marching
{
    public class AttackingArmy
    {
        private List<AttackingUnit> units = new List<AttackingUnit>();


        public int UnitsCount => units.Count;


        public void AddUnit(Unit unit, Settlment settlment, int marchingTime) =>
            units.Add(new AttackingUnit(unit, settlment, marchingTime));

        public void RemoveUnit(AttackingUnit unit) => units.Remove(unit);

        public List<AttackingUnit> GetUnits() => new List<AttackingUnit>(units);
    }
}
