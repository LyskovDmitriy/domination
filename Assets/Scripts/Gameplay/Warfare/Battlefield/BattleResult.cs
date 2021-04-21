using Domination.Warfare;
using System.Collections.Generic;


namespace Domination.Battle.Logic
{
    public class BattleResult
    {
        public bool wasSiegeSuccessful;
        public HashSet<Unit> killedAttackingUnits;
        public HashSet<Unit> killedDefendingingUnits;
    }
}
