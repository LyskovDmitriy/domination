using Domination.Warfare;


namespace Domination
{
    public static class RecruitmentUtils
    {
        public static int UnitPrice => UnitRecruitmentSettings.UnitPrice;

        public static int GetHealth(WeaponType type) => UnitRecruitmentSettings.GetHealth(type);

        public static Unit CreateUnit(WeaponType weaponType, int weaponLevel) =>
            new Unit(weaponLevel, weaponType, GetHealth(weaponType));

        public static Army CreateArmy(UnitsGroup[] groups)
        {
            var army = new Army();

            foreach (var group in groups)
            {
                for (int i = 0; i < group.UnitsCount; i++)
                {
                    army.AddUnit(CreateUnit(group.WeaponType, group.WeaponLevel));
                }
            }

            return army;
        }
    }
}
