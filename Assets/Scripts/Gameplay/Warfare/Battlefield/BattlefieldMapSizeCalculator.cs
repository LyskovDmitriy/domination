using Domination.Battle.Settings;
using System;
using System.Linq;
using UnityEngine;
using Utils;


namespace Domination.Battle.Logic
{
    public static class BattlefieldMapSizeCalculator
    {
        public static Vector2Int CalculateMapSize(int attackingUnitsCount, int defendingUnitsCount, int wallThickness, int minWallHeight)
        {
            var attackersMapSize = CalculateMapSizeBasedOnAttackers(attackingUnitsCount, wallThickness);
            var defendersMapSize = CalculateMapSizeBasedOnDefenders(defendingUnitsCount, wallThickness);
            var minSize = 
                new Vector2Int(
                    Mathf.CeilToInt(minWallHeight * BattleFieldSettings.TagetBattleFieldSizeRatio), 
                    minWallHeight);

            return Vector2Int.Max(Vector2Int.Max(attackersMapSize, defendersMapSize), minSize);
        }

        public static Vector2Int CalculateMapSizeBasedOnAttackers(int attackingUnitsCount, int wallThickness)
        {
            var roots = MathUtils.SolveQuadraticEquasion(
                a: BattleFieldSettings.TagetBattleFieldSizeRatio,
                b: -(2 * BattleFieldSettings.AttackersMinDistanceFromWall + wallThickness),
                c: -2 * attackingUnitsCount);

            int height = Mathf.RoundToInt(roots.Max());
            int width = 2 * (Mathf.CeilToInt((float)attackingUnitsCount / height) + BattleFieldSettings.AttackersMinDistanceFromWall) + wallThickness;

            return new Vector2Int(width, height);
        }

        public static Vector2Int CalculateMapSizeBasedOnDefenders(int defendingUnitsCount, int wallThickness)
        {
            var roots = MathUtils.SolveQuadraticEquasion(
                a: BattleFieldSettings.TagetBattleFieldSizeRatio,
                b: -wallThickness,
                c: -2 * defendingUnitsCount);

            int height = Mathf.RoundToInt(roots.Max());
            int width = 2 * (Mathf.CeilToInt((float)defendingUnitsCount / height)) + wallThickness;

            return new Vector2Int(width, height);
        }
    }
}
