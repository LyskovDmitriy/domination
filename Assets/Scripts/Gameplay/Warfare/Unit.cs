namespace Domination.Warfare
{
    public class Unit 
    {
        public WeaponType WeaponType { get; private set; }

        private Weapon weapon;
        private int health;

        public Unit(Weapon weapon, WeaponType weaponType, int health)
        {
            this.weapon = weapon;
            this.health = health;
            WeaponType = weaponType;
        }
    }
}
