using System.Collections.Generic;


namespace Domination.Battle.Logic
{
    public class ArmyCommander
    {
        private List<Warrior> warriors = new List<Warrior>();


        public void AddWarrior(Warrior warrior)
        {
            warriors.Add(warrior);
        }
    }
}
