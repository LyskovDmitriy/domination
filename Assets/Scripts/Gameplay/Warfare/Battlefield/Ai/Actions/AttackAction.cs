namespace Domination.Battle.Logic.Ai
{
    public class AttackAction : IAction
    {
        private readonly IMapUnit target;
        private readonly int damage;


        public AttackAction(Warrior warrior, IMapUnit target)
        {
            damage = warrior.Unit.Weapon.Damage;
            this.target = target;
        }

        public void Execute(Warrior warrior) => target.ReceiveDamage(damage);
    }
}
