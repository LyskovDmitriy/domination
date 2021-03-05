using System;


namespace Domination.Data
{
    [Serializable]
    public class CharacterData
    {
        public uint id;
        public bool isPlayer;
        public int coinsCount;

        public int meleeWeaponLevel;
        public int rangedWeaponLevel;

        public uint[] ownedSettlments;

        public StationedArmyData[] stationedArmies;
        public MarchingUnitData[] marchingUnits;
    }
}
