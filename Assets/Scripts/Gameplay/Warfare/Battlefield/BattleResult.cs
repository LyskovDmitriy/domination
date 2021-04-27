using Domination.Warfare;
using System.Collections.Generic;


namespace Domination.Battle.Logic
{
    public class BattleResult
    {
        public bool wasSiegeSuccessful;
        public HashSet<Unit> attackerCasualties;
        public HashSet<Unit> defenderCasualties;
    }
}
