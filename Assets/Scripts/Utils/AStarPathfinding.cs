using System;
using System.Collections.Generic;
using UnityEngine;


namespace Utils
{
    public static class AStarPathfinding
    {
        private class Node
        {
            public Vector2Int position;
            public Vector2Int previousPosition;
            public float distance;


            public Node(Vector2Int position, Vector2Int previousPosition, float distance)
            {
                this.position = position;
                this.previousPosition = previousPosition;
                this.distance = distance;
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

        public static float GetDistance<TTileType>(
            Vector2Int start, 
            Vector2Int target, 
            TTileType[,] map, 
            Func<TTileType, float> getPassingCostAction, 
            float distanceHeuristicsModifier) //TODO: Create test chamber
        {
            Node[,] visitedPositions = new Node[map.GetLength(0), map.GetLength(1)];
            SortedList<int, Queue<Node>> frontier = new SortedList<int, Queue<Node>>() { { 0, new Queue<Node>() } };
            frontier[0].Enqueue(new Node(start, start, 0));

            while (frontier.Count > 0)
            {
                var positions = frontier.Values[0];
                Node currentNode = positions.Dequeue();

                if (positions.Count == 0)
                {
                    frontier.RemoveAt(0);
                }

                if (currentNode.position == target)
                {
                    return currentNode.distance;
                }

                foreach (var offset in NeighborsOffsets)
                {
                    Vector2Int newNodePosition = currentNode.position + offset;

                    if ((0 <= newNodePosition.x) && (newNodePosition.x < map.GetLength(0)) &&
                        (0 <= newNodePosition.y) && (newNodePosition.y < map.GetLength(1)))
                    {
                        float distance = currentNode.distance + getPassingCostAction.Invoke(map[newNodePosition.x, newNodePosition.y]);
                        Node newNode = null;

                        if (visitedPositions[newNodePosition.x, newNodePosition.y] == null)
                        {
                            newNode = new Node(newNodePosition, currentNode.position, distance);
                            visitedPositions[newNodePosition.x, newNodePosition.y] = newNode;
                        }
                        else
                        {
                            Node existingNode = visitedPositions[newNodePosition.x, newNodePosition.y];

                            if (distance < existingNode.distance)
                            {
                                existingNode.distance = distance;
                                existingNode.previousPosition = currentNode.position;
                            }
                        }

                        if (newNode != null)
                        {
                            int heuristicDistance = GetHeuristicDistance(newNodePosition, target, distance, distanceHeuristicsModifier);

                            if (!frontier.TryGetValue(heuristicDistance, out var queue))
                            {
                                queue = new Queue<Node>();
                                frontier.Add(heuristicDistance, queue);
                            }

                            queue.Enqueue(newNode);
                        }
                    }
                }
            }

            return int.MaxValue;
        }

        private static int GetHeuristicDistance(
            Vector2Int position, 
            Vector2Int targetPosition, 
            float distance, 
            float distanceHeuristicsModifier) =>
            Mathf.Abs(position.x - targetPosition.x) + Mathf.Abs(position.y - targetPosition.y) + Mathf.RoundToInt(distance * distanceHeuristicsModifier);
    }
}
