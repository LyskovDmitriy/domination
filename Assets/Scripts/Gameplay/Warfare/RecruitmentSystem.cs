using Domination.LevelLogic;
using Domination.Warfare;
using System;

namespace Domination
{
    public class RecruitmentSystem
    {
        private readonly Func<uint, Settlment> getSettlmentAction;


        public int UnitPrice => UnitRecruitmentSettings.UnitPrice;


        public RecruitmentSystem(Func<uint, Settlment> getSettlmentAction) => 
            this.getSettlmentAction = getSettlmentAction;

        public int GetHealth(WeaponType type) => UnitRecruitmentSettings.GetHealth(type);

        public bool CanRecruit(uint settlmentId) => 
            GetSettlmentLord(settlmentId).HasCoins(UnitPrice);

        public void Recruit(uint settlmentId, WeaponType weaponType, int weaponLevel)
        {
            Unit unit = new Unit(weaponLevel, weaponType, GetHealth(weaponType));
            GetSettlmentLord(settlmentId).Recruit(unit, settlmentId);
        }

        public void SetupNeutralVillageArmy(Village village)
        {
            foreach (var group in SettlmentsSettings.NeutralVillageArmy)
            {
                for (int i = 0; i < group.UnitsCount; i++)
                {
                    village.Recruit(CreateUnit(group.WeaponType, group.WeaponLevel));
                }
            }
        }

        private Unit CreateUnit(WeaponType weaponType, int weaponLevel) => 
            new Unit(weaponLevel, weaponType, GetHealth(weaponType));

        private Character GetSettlmentLord(uint settlmentId) =>
            getSettlmentAction(settlmentId).Lord;
    }
}
