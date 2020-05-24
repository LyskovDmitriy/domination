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


        public Character GetCharacterWithSettlment(int settlmentId) => Array.Find(level.Characters, character => character.HasSettlment(settlmentId));

        public Character GetCharacterById(int id) => Array.Find(level.Characters, character => character.Id == id);

        public Settlment GetSettlment(int settlmentId)
        {
            return GetCharacterWithSettlment(settlmentId).GetSettlmentById(settlmentId);
        }
    }
}
