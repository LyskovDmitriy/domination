using Domination.LevelLogic;
using Domination.Warfare;
using System.Collections.Generic;
using UnityEngine;


namespace Domination.Battle.Logic
{
    public class BattleController
    {
        public readonly Vector2Int BattleFieldSize;

        public BattleController(Army attackingArmy, Army defendingArmy, Settlment settlment, Tile tile)
        {
            int wallThickness = 0;
            int minWallHeight = 0;

            var wall = settlment.GetBuilding(BuildingType.Wall);
            if (wall != null)
            {
                var settings = SettlmentsSettings.GetBuildingInfo<WallSettings>();
                var wallInfo = settings.Levels[wall.level];
                wallThickness = wallInfo.WallThickness;
                minWallHeight = wallInfo.MinWallHeight;
            }

            BattleFieldSize = BattlefieldMapSizeCalculator.CalculateMapSize(
                attackingArmy.TotalUnitsCount, 
                defendingArmy.TotalUnitsCount, 
                wallThickness,
                minWallHeight);
        }
    }
}
