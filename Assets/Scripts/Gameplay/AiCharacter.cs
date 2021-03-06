using Domination.Data;
using Domination.LevelLogic;
using System;

namespace Domination
{
    public class AiCharacter : Character
    {
        public AiCharacter() : base() { }

        public AiCharacter(Func<uint, Settlment> settlmentGetter, CharacterData data) :
            base(settlmentGetter, data) { }

        public override void StartTurn(bool isFirstTurn)
        {
            base.StartTurn(isFirstTurn);

            FinishTurn();
        }
    }
}
