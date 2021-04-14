namespace Domination.Battle.Logic.Ai
{
    public interface IUnitPlanner
    {
        IAction PlannedAction { get; }

        void PlanTurn(IMapUnit[,] planningMap, BattlePathfiner.PathfinidingResult pathfinidingResult);
    }
}
