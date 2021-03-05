using Domination.LevelLogic;
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

        public static int GetHealth(WeaponType type) => UnitRecruitmentSettings.GetHealth(type);


        public static bool CanRecruit(uint settlmentId)
        {
            return levelWrapper.GetCharacterWithSettlment(settlmentId).HasCoins(UnitPrice);
        }


        public static void Recruit(uint settlmentId, WeaponType weaponType, int weaponLevel)
        {
            Unit unit = new Unit(weaponLevel, weaponType, GetHealth(weaponType));

            levelWrapper.GetCharacterWithSettlment(settlmentId).Recruit(unit, settlmentId);
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


        private static Unit CreateUnit(WeaponType weaponType, int weaponLevel)
        {
            return new Unit(weaponLevel, weaponType, GetHealth(weaponType));
        }
    }
}
