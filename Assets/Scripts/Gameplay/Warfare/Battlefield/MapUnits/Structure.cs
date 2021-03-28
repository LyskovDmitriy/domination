using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Domination.Battle.Logic
{
    public class Structure : IMapUnit
    {
        public readonly bool isGate;

        
        public MapUnitType Type => MapUnitType.Structure;


        public Structure(bool isGate)
        {
            this.isGate = isGate;
        }
    }
}
