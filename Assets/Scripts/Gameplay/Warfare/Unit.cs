using Domination.Data;


namespace Domination.Warfare
{
    public class Unit 
    {
        public WeaponType WeaponType { get; private set; }
        public int WeaponLevel { get; private set; }
        public Weapon Weapon => UnitRecruitmentSettings.GetWeapons(WeaponType)[WeaponLevel].Weapon;

        public int Health { get; private set; }

        
        public Unit(int weaponLevel, WeaponType weaponType, int health)
        {
            WeaponType = weaponType;
            WeaponLevel = weaponLevel;
            Health = health;
        }

        public Unit(Unit unit)
        {
            WeaponType = unit.WeaponType;
            WeaponLevel = unit.WeaponLevel;
            Health = unit.Health;
        }

        public Unit(UnitData data)
        {
            WeaponType = data.weaponType;
            WeaponLevel = data.weaponLevel;
            Health = data.health;
        }

        public UnitData GetData() => new UnitData
        {
            weaponType = WeaponType,
            weaponLevel = WeaponLevel,
            health = Health
        };

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Unit unit))
            {
                return false;
            }

            return (WeaponType == unit.WeaponType) && 
                (WeaponLevel == unit.WeaponLevel) &&
                (Health == unit.Health);
        }
    }
}
