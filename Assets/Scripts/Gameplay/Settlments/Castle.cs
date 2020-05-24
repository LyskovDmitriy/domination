namespace Domination
{
    public class Castle : Settlment
    {
        public override SettlmentType Type => SettlmentType.Castle;


        protected override void Awake()
        {
            base.Awake();

            foreach (var buildingInfo in SettlmentsSettings.AvailableBuildingsInCity)
            {
                for (int i = 0; i < buildingInfo.defaultLevel; i++)
                {
                    if (i == 0)
                    {
                        Build(buildingInfo.type);
                    }
                    else
                    {
                        UpgradeBuilding(buildingInfo.type);
                    }
                }
            }
        }
    }
}
