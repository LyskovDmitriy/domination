using Domination.Warfare;


namespace Domination
{
    public static class RecruitmentUtils
    {
        public static int UnitPrice => UnitRecruitmentSettings.UnitPrice;

        public static int GetHealth(WeaponType type) => UnitRecruitmentSettings.GetHealth(type);

        public static Unit CreateUnit(WeaponType weaponType, int weaponLevel) =>
            new Unit(weaponLevel, weaponType, GetHealth(weaponType));
    }
}
