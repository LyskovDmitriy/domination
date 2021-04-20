using System.Collections.Generic;
using System.Linq;


namespace Domination.Battle.Logic.Ai
{
    public class MeleeUnitPlanner : ITurnPlanner
    {
        private readonly Warrior warrior;

        private IMapUnit[,] currentPlanningMap;


        public IAction PlannedAction { get; private set; }

        public IMapUnit CurrentTarget { get; private set; }


        public MeleeUnitPlanner(Warrior warrior)
        {
            this.warrior = warrior;
        }

        public void PlanTurn(IMapUnit[,] planningMap, BattlePathfiner.PathfinidingResult pathfinidingResult)
        {
            currentPlanningMap = planningMap;
            var warriorsToAttack = pathfinidingResult.enemies.Where(CanNodeBeReached);

            //Try to attack warriors that can be reached
            if (warriorsToAttack.Count() > 0)
            {
                SelectClosestTarget(warriorsToAttack);
                return;
            }

            if (warrior.IsAttacker)
            {
                var structuresToAttack = pathfinidingResult.structures.Where(CanNodeBeReached);

                //Try to attack structures that can be reached
                if (structuresToAttack.Count() > 0)
                {
                    SelectClosestTarget(structuresToAttack);
                    return;
                }
            }

            //Try to attack closest warrior
            SelectClosestTarget(pathfinidingResult.enemies);
        }

        public void ExecuteTurn()
        {
            PlannedAction.Execute(warrior);
            PlannedAction = null;
        }

        private void SelectClosestTarget(IEnumerable<BattlePathfiner.Node> possibleTargets)
        {
            List<BattlePathfiner.Node> closestTargets = new List<BattlePathfiner.Node>();
            int minDistance = int.MaxValue;

            foreach (var target in possibleTargets)
            {
                if (minDistance == target.previousNode.distance)
                {
                    closestTargets.Add(target);
                }
                else if (target.previousNode.distance < minDistance)
                {
                    minDistance = target.previousNode.distance;
                    closestTargets.Clear();
                    closestTargets.Add(target);
                }
            }

            if (CurrentTarget != null)
            {
                var previousTargetNode = closestTargets.Find(node => currentPlanningMap[node.position.x, node.position.y] == CurrentTarget);

                if (previousTargetNode != null)
                {
                    SelectTarget(previousTargetNode);
                    return;
                }
            }

            var node = closestTargets.RandomObject();
            SelectTarget(node);
        }

        private void SelectTarget(BattlePathfiner.Node targetNode)
        {
            CurrentTarget = currentPlanningMap[targetNode.position.x, targetNode.position.y];

            var currentNode = targetNode;

            while (currentNode.previousNode.position != warrior.Position)
            {
                currentNode = currentNode.previousNode;
            }

            if (currentNode == targetNode)
            {
                PlannedAction = new AttackAction(CurrentTarget);
            }
            else if (currentPlanningMap[currentNode.position.x, currentNode.position.y] == null)
            {
                PlannedAction = new MoveAction(currentNode.position);

                currentPlanningMap[warrior.Position.x, warrior.Position.y] = null;
                currentPlanningMap[currentNode.position.x, currentNode.position.y] = warrior;
            }
            else
            {
                PlannedAction = new IdleAction();
            }
        }

        private bool CanNodeBeReached(BattlePathfiner.Node node) =>
            !node.previousNode.isPathObstructedByStructure && !node.previousNode.isPathObstructedByWarrior;
    }
}
