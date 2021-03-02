using Domination.LevelLogic;
using System;


namespace Domination.Utils
{
    public class LevelWrapper
    {
        private Level level;

        public LevelWrapper(Level level)
        {
            this.level = level;
        }


        public Character GetCharacterWithSettlment(uint settlmentId) => Array.Find(level.Characters, character => character.HasSettlment(settlmentId));

        public Character GetCharacterById(uint id) => Array.Find(level.Characters, character => character.Id == id);

        public Settlment GetSettlment(uint settlmentId)
        {
            return GetCharacterWithSettlment(settlmentId).GetSettlmentById(settlmentId);
        }
    }
}
