namespace Domination.Warfare
{
    public class Unit 
    {
        public WeaponType WeaponType { get; private set; }

        private Weapon weapon;

        public Unit(Weapon weapon, WeaponType weaponType)
        {
            this.weapon = weapon;
            WeaponType = weaponType;
        }
    }
}
