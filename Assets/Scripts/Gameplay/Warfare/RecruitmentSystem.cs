using Domination.LevelLogic;
using Domination.Warfare;


namespace Domination
{
    public static class RecruitmentSystem
    {
        private static Level level;


        public static int UnitPrice => UnitRecruitmentSettings.UnitPrice;


        public static void Init(Level currentLevel) => level = currentLevel;

        public static int GetHealth(WeaponType type) => UnitRecruitmentSettings.GetHealth(type);

        public static bool CanRecruit(uint settlmentId) => 
            GetSettlmentLord(settlmentId).HasCoins(UnitPrice);

        public static void Recruit(uint settlmentId, WeaponType weaponType, int weaponLevel)
        {
            Unit unit = new Unit(weaponLevel, weaponType, GetHealth(weaponType));
            GetSettlmentLord(settlmentId).Recruit(unit, settlmentId);
        }

        public static void SetupNeutralVillageArmy(Village village)
        {
            foreach (var group in SettlmentsSettings.NeutralVillageArmy)
            {
                for (int i = 0; i < group.UnitsCount; i++)
                {
                    village.Recruit(CreateUnit(group.WeaponType, group.WeaponLevel));
                }
            }
        }

        private static Unit CreateUnit(WeaponType weaponType, int weaponLevel) => 
            new Unit(weaponLevel, weaponType, GetHealth(weaponType));

        private static Character GetSettlmentLord(uint settlmentId) =>
            level.GetSettlment(settlmentId).Lord;
    }
}
