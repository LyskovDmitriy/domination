using Domination.Data;
using System.Linq;
using System.Collections.Generic;


namespace Domination.Warfare
{
    public class Army
    {
        private List<Unit> meleeGroup;
        private List<Unit> rangedGroup;


        public int TotalUnitsCount => meleeGroup.Count + rangedGroup.Count;

        public bool IsEmpty => (TotalUnitsCount == 0);


        public Army(Army armyToCopy)
        {
            meleeGroup = new List<Unit>(armyToCopy.meleeGroup);
            rangedGroup = new List<Unit>(armyToCopy.rangedGroup);
        }

        public Army()
        {
            meleeGroup = new List<Unit>();
            rangedGroup = new List<Unit>();
        }


        public int GetUnitsCount(WeaponType weaponType) => GetGroup(weaponType).Count;

        public void AddUnits(IEnumerable<Unit> units)
        {
            foreach (var unit in units)
            {
                AddUnit(unit);
            }
        }

        public void AddUnit(Unit unit)
        {
            GetGroup(unit.WeaponType).Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            GetGroup(unit.WeaponType).Remove(unit);
        }

        public List<Unit> GetUnits()
        {
            List<Unit> units = new List<Unit>(meleeGroup.Count + rangedGroup.Count);
            units.AddRange(meleeGroup);
            units.AddRange(rangedGroup);
            return units;
        }

        public void Clear()
        {
            meleeGroup.Clear();
            rangedGroup.Clear();
        }

        public UnitData[] GetData() => meleeGroup.Concat(rangedGroup)
            .Select(unit => unit.GetData()).ToArray();

        private List<Unit> GetGroup(WeaponType weaponType) => (weaponType == WeaponType.Melee) ? meleeGroup : rangedGroup;
    }
}
