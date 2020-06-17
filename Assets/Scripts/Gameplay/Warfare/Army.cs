using System.Collections.Generic;


namespace Domination.Warfare
{
    public class Army
    {
        private List<Unit> meleeGroup = new List<Unit>();
        private List<Unit> rangedGroup = new List<Unit>();


        public int GetUnitsCount(WeaponType weaponType) => GetGroup(weaponType).Count;


        public void AddUnit(Unit unit)
        {
            GetGroup(unit.WeaponType).Add(unit);
        }


        public List<Unit> GetUnits()
        {
            List<Unit> units = new List<Unit>(meleeGroup.Count + rangedGroup.Count);
            units.AddRange(meleeGroup);
            units.AddRange(rangedGroup);
            return units;
        }


        private List<Unit> GetGroup(WeaponType weaponType) => (weaponType == WeaponType.Melee) ? meleeGroup : rangedGroup;
    }
}
