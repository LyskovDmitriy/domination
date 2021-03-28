using Domination.Battle.Settings;
using Domination.LevelLogic;
using Domination.Warfare;
using System.Collections.Generic;
using UnityEngine;


namespace Domination.Battle.Logic
{
    public class BattleController
    {
        public readonly Vector2Int BattleFieldSize;
        public readonly IMapUnit[,] MapUnits;


        public BattleController(Army attackingArmy, Army defendingArmy, Settlment settlment, Tile tile)
        {
            int wallThickness = 0;
            int minWallHeight = 0;
            int gateHeight = 0;

            var wall = settlment.GetBuilding(BuildingType.Wall);
            if (wall != null)
            {
                var settings = SettlmentsSettings.GetBuildingInfo<WallSettings>();
                var wallInfo = settings.Levels[wall.level];
                wallThickness = wallInfo.WallThickness;
                minWallHeight = wallInfo.MinWallHeight;
                gateHeight = wallInfo.GateHeight;
            }

            BattleFieldSize = BattlefieldMapSizeCalculator.CalculateMapSize(
                attackingArmy.TotalUnitsCount, 
                defendingArmy.TotalUnitsCount, 
                wallThickness,
                minWallHeight);

            MapUnits = new IMapUnit[BattleFieldSize.x, BattleFieldSize.y];

            CreateWall(wallThickness, gateHeight);
            CreateUnits(attackingArmy.GetUnits(), true, wallThickness);
        }

        private void CreateWall(int wallThickness, int gateHeight)
        {
            int wallStartingIndex = (BattleFieldSize.x - wallThickness) / 2;
            int gateStartingIndex = (BattleFieldSize.y - gateHeight) / 2;

            for (int x = wallStartingIndex; x < wallStartingIndex + wallThickness; x++)
            {
                for (int y = 0; y < BattleFieldSize.y; y++)
                {
                    bool isGate = ((gateStartingIndex <= y) && (y < gateStartingIndex + gateHeight));
                    MapUnits[x, y] = new Structure(isGate);
                }
            }
        }

        private void CreateUnits(List<Unit> units, bool isAttacker, int wallThickness)
        {
            var queue = new Queue<Unit>(units);
            int placementDirectionSign = isAttacker ? -1 : 1;
            int startingColumnIndex = (BattleFieldSize.x - wallThickness) / 2 + 
                (isAttacker ? -(1 + BattleFieldSettings.AttackersMinDistanceFromWall) :  wallThickness);

            for (int x = startingColumnIndex; (0 <= x) && (x < BattleFieldSize.x); x += placementDirectionSign)
            {
                for (int offsetY = 0; IsWithinBounds(x, startingColumnIndex - offsetY) || IsWithinBounds(x, startingColumnIndex + offsetY); offsetY++)
                {
                    int centerIndexY = Mathf.FloorToInt(BattleFieldSize.y / 2.0f);
                    TryAddUnit(x, centerIndexY + offsetY, queue);

                    if (offsetY > 0)
                    {
                        TryAddUnit(x, centerIndexY - offsetY, queue);
                    }
                    if (units.Count == 0)
                    {
                        return;
                    }
                }
            }
        }

        private void TryAddUnit(int x, int y, Queue<Unit> units)
        {
            if ((units.Count > 0) && IsWithinBounds(x, y))
            {
                var warrior = new Warrior(units.Dequeue());
                MapUnits[x, y] = warrior;
            }
        }

        private bool IsWithinBounds(int x, int y)
        {
            return (0 <= x) && (x < BattleFieldSize.x) &&
                (0 <= y) && (y < BattleFieldSize.y);
        }
    }
}
