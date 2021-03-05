using System;


namespace Domination.Data
{
    [Serializable]
    public class LevelData
    {
        public int activeCharacterIndex;
        public int currentTurn;

        public CharacterData[] actingCharacters;
        public CharacterData neutralCharacter;

        public MapData mapData;
    }
}
