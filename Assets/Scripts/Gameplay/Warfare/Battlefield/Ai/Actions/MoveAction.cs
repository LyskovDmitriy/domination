using UnityEngine;


namespace Domination.Battle.Logic.Ai
{
    public class MoveAction : IAction
    {
        public readonly Vector2Int TargetPosition;


        public MoveAction(Vector2Int targetPosition)
        {
            TargetPosition = targetPosition;
        }
    }
}
