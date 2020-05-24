using Domination.Utils;
using Domination.Warfare;


namespace Domination
{
    public static class RecruitmentSystem
    {
        private static LevelWrapper levelWrapper;


        public static int UnitPrice => UnitRecruitmentSettings.UnitPrice;


        public static void Init(LevelWrapper currentLevel)
        {
            levelWrapper = currentLevel;
        }


        public static WeaponInfo GetWeaponInfo(WeaponType type, int level)
        {
            return UnitRecruitmentSettings.GetWeapons(type)[level];
        }

        public static int GetHealth(WeaponType type) => UnitRecruitmentSettings.GetHealth(type);


        public static bool CanRecruit(int settlmentId)
        {
            return levelWrapper.GetCharacterWithSettlment(settlmentId).HasCoins(UnitPrice);
        }


        public static void Recruit(int settlmentId, WeaponType weaponType, int level)
        {
            WeaponInfo weapon = GetWeaponInfo(weaponType, level);
            Unit unit = new Unit(weapon.Weapon, weaponType);

            levelWrapper.GetCharacterWithSettlment(settlmentId).Recruit(unit, settlmentId);
        }
    }
}
