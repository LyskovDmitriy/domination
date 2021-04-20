namespace Domination.Battle.Logic.Ai
{
    public class AttackAction : IAction
    {
        private readonly IMapUnit target;


        public AttackAction(IMapUnit target)
        {
            this.target = target;
        }

        public void Execute(Warrior warrior) { }
    }
}
