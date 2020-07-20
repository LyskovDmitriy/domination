namespace Domination
{
    public class AiCharacter : Character
    {
        public AiCharacter() : base() { }

        public override void StartTurn(bool isFirstTurn)
        {
            base.StartTurn(isFirstTurn);

            FinishTurn();
        }
    }
}
