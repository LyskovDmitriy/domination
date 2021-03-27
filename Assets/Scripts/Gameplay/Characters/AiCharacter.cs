using Domination.Data;
using Domination.EventsSystem;
using Domination.LevelLogic;
using System;


namespace Domination
{
    public class AiCharacter : Character
    {
        public AiCharacter(EventsAggregator aggregator) : base(aggregator) { }

        public AiCharacter(EventsAggregator aggregator, Func<uint, Settlment> settlmentGetter, CharacterData data) :
            base(aggregator, settlmentGetter, data) { }

        public override void StartTurn(bool isFirstTurn)
        {
            base.StartTurn(isFirstTurn);

            FinishTurn();
        }
    }
}
