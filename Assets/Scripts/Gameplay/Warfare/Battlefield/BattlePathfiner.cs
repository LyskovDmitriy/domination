using System;
using System.Collections.Generic;
using UnityEngine;


namespace Domination.Battle.Logic
{
    public static class BattlePathfiner
    {
        public class Node
        {
            public Vector2Int position;
            public Vector2Int previousPosition;
            public bool isPathObstructedByStructure;
            public bool isPathObstructedByWarrior;
            public int distance;


            public Node(
                Vector2Int position, 
                Vector2Int previousPosition, 
                int distance, 
                bool isPathObstructedByStructure, 
                bool isPathObstructedByWarrior)
            {
                this.position = position;
                this.previousPosition = previousPosition;
                this.distance = distance;
                this.isPathObstructedByStructure = isPathObstructedByStructure;
                this.isPathObstructedByWarrior = isPathObstructedByWarrior;
            }

            public override bool Equals(object obj)
            {
                return position.Equals((obj as Node).position);
            }

            public override int GetHashCode()
            {
                return position.GetHashCode();
            }
        }

        private static readonly Vector2Int[] NeighborsOffsets = new Vector2Int[]
        {   new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1)
        };


        public static Node[,] GetPathfindingData(
            Vector2Int start,
            IMapUnit[,] map,
            Func<IMapUnit, int> getPassingCostAction)
        {
            var visitedPositions = new Node[map.GetLength(0), map.GetLength(1)];
            var frontier = new SortedList<int, Queue<Node>>() { { 0, new Queue<Node>() } };

            var startingNode = new Node(start, start, 0, false, false);
            frontier[0].Enqueue(startingNode);
            visitedPositions[start.x, start.y] = startingNode;

            while (frontier.Count > 0)
            {
                var positions = frontier.Values[0];
                Node currentNode = positions.Dequeue();

                if (positions.Count == 0)
                {
                    frontier.RemoveAt(0);
                }

                foreach (var offset in NeighborsOffsets)
                {
                    Vector2Int newNodePosition = currentNode.position + offset;

                    if ((0 <= newNodePosition.x) && (newNodePosition.x < map.GetLength(0)) &&
                        (0 <= newNodePosition.y) && (newNodePosition.y < map.GetLength(1)))
                    {
                        var unitOnTile = map[newNodePosition.x, newNodePosition.y];
                        int distance = currentNode.distance + getPassingCostAction.Invoke(unitOnTile);

                        bool isMapUnitStructure = (unitOnTile != null) && (unitOnTile.Type == MapUnitType.Structure);
                        bool isMapUnitWarrior = (unitOnTile != null) && (unitOnTile.Type == MapUnitType.Warrior);

                        if (visitedPositions[newNodePosition.x, newNodePosition.y] == null)
                        {
                            var newNode = new Node(
                                newNodePosition, 
                                currentNode.position, 
                                distance,
                                isMapUnitStructure,
                                isMapUnitWarrior);
                            visitedPositions[newNodePosition.x, newNodePosition.y] = newNode;

                            if (!frontier.TryGetValue(distance, out var queue))
                            {
                                queue = new Queue<Node>();
                                frontier.Add(distance, queue);
                            }

                            queue.Enqueue(newNode);
                        }
                        else
                        {
                            Node existingNode = visitedPositions[newNodePosition.x, newNodePosition.y];

                            if (distance < existingNode.distance)
                            {
                                existingNode.isPathObstructedByStructure = isMapUnitStructure || currentNode.isPathObstructedByStructure;
                                existingNode.isPathObstructedByWarrior = isMapUnitWarrior || currentNode.isPathObstructedByWarrior;
                                existingNode.distance = distance;
                                existingNode.previousPosition = currentNode.position;
                            }
                        }
                    }
                }
            }

            return visitedPositions;
        }
    }
}
