using Domination.Warfare;
using System.Collections.Generic;
using UnityEngine;


namespace Domination.Battle.Logic
{
    public class Warrior : IMapUnit
    {
        public readonly Unit Unit;


        public MapUnitType Type => MapUnitType.Warrior;


        public Warrior(Unit unit)
        {
            Unit = unit;
        }
    }
}
