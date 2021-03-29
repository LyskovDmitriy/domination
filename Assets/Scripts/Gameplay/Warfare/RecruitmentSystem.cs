using Domination.LevelLogic;
using Domination.Warfare;
using System;


namespace Domination
{
    public class RecruitmentSystem
    {
        private readonly Func<uint, Settlment> getSettlmentAction;


        public RecruitmentSystem(Func<uint, Settlment> getSettlmentAction) => 
            this.getSettlmentAction = getSettlmentAction;

        public bool CanRecruit(uint settlmentId) => 
            GetSettlmentLord(settlmentId).HasCoins(RecruitmentUtils.UnitPrice);

        public void Recruit(uint settlmentId, WeaponType weaponType, int weaponLevel)
        {
            Unit unit = RecruitmentUtils.CreateUnit(weaponType, weaponLevel);
            GetSettlmentLord(settlmentId).Recruit(unit, settlmentId);
        }

        public void SetupNeutralVillageArmy(Village village)
        {
            foreach (var group in SettlmentsSettings.NeutralVillageArmy)
            {
                for (int i = 0; i < group.UnitsCount; i++)
                {
                    village.Recruit(RecruitmentUtils.CreateUnit(group.WeaponType, group.WeaponLevel));
                }
            }
        }

        private Character GetSettlmentLord(uint settlmentId) =>
            getSettlmentAction(settlmentId).Lord;
    }
}
