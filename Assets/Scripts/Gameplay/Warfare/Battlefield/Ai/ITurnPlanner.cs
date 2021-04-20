namespace Domination.Battle.Logic.Ai
{
    public interface ITurnPlanner
    {
        IAction PlannedAction { get; }
        IMapUnit CurrentTarget { get; }

        void PlanTurn(IMapUnit[,] planningMap, BattlePathfiner.PathfinidingResult pathfinidingResult);
        void ExecuteTurn();
    }
}
