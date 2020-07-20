namespace Domination.Warfare
{
    public class Unit 
    {
        public WeaponType WeaponType { get; private set; }
        public Weapon Weapon { get; private set; }
        public int Health { get; set; }

        
        public Unit(Weapon weapon, WeaponType weaponType, int health)
        {
            Weapon = weapon;
            Health = health;
            WeaponType = weaponType;
        }

        public Unit(Unit unit)
        {
            WeaponType = unit.WeaponType;
            Weapon = unit.Weapon;
            Health = unit.Health;
        }
    }
}
