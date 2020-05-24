namespace Domination
{
    public class AiCharacter : Character
    {
        public AiCharacter(Castle castle) : base(castle) { }

        public override void StartTurn(bool isFirstTurn)
        {
            base.StartTurn(isFirstTurn);

            FinishTurn();
        }
    }
}
