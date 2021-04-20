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

        private readonly ArmyCommander[] commanders;

        private List<IMapUnit> structures = new List<IMapUnit>();


        public BattleController(Army attackingArmy, Army defendingArmy, Settlment settlment, Tile tile)//TODO: Improve walls  health on a mountain tile
        {
            int wallThickness = 0;
            int minWallHeight = 0;
            int wallHealth = 0;

            int gateHeight = 0;
            int gateHealth = 0;

            var wall = settlment.GetBuilding(BuildingType.Wall);
            if (wall != null)
            {
                var settings = SettlmentsSettings.GetBuildingInfo<WallSettings>();
                var wallInfo = settings.Levels[wall.level];
                wallThickness = wallInfo.WallThickness;
                minWallHeight = wallInfo.MinWallHeight;
                wallHealth = wallInfo.WallUnitHealth;

                gateHeight = wallInfo.GateHeight;
                gateHealth = wallInfo.GateUnitHealth;
            }

            BattleFieldSize = BattlefieldMapSizeCalculator.CalculateMapSize(
                attackingArmy.TotalUnitsCount, 
                defendingArmy.TotalUnitsCount, 
                wallThickness,
                minWallHeight);

            commanders = new ArmyCommander[]
            {
                new ArmyCommander(this, isAttacker: true),
                new ArmyCommander(this, isAttacker: false),
            };

            MapUnits = new IMapUnit[BattleFieldSize.x, BattleFieldSize.y];

            CreateWall(wallThickness, gateHeight, wallHealth, gateHealth);

            CreateUnits(commanders[0], attackingArmy.GetUnits(), true, wallThickness);
            CreateUnits(commanders[1], defendingArmy.GetUnits(), false, wallThickness);
        }

        public void PlanTurn()
        {
            var mapForPlanning = (IMapUnit[,])MapUnits.Clone();

            foreach (var commander in commanders)
            {
                commander.PlanTurn(mapForPlanning);
            }
        }

        public void ExecutePlan()
        {
            foreach (var commander in commanders)
            {
                commander.ExecuteTurn();
            }

            for (int x = 0; x < BattleFieldSize.x; x++)
            {
                for (int y = 0; y < BattleFieldSize.y; y++)
                {
                    MapUnits[x, y] = null;
                }
            }

            AddMapUnits(structures);

            foreach (var commander in commanders)
            {
                AddMapUnits(commander.Warriors);
            }

            void AddMapUnits(IEnumerable<IMapUnit> mapUnits)
            {
                foreach (var unit in mapUnits)
                {
                    MapUnits[unit.Position.x, unit.Position.y] = unit;
                }
            }
        }

        public bool IsTileEmpty(IMapUnit[,] map, Vector2Int position) => IsTileEmpty(map, position.x, position.y);
        public bool IsTileEmpty(IMapUnit[,] map, int x, int y) => IsWithinBounds(x, y) && (map[x, y] == null);

        private void CreateWall(int wallThickness, int gateHeight, int wallHealth, int gateHealth)
        {
            int wallStartingIndex = (BattleFieldSize.x - wallThickness) / 2;
            int gateStartingIndex = (BattleFieldSize.y - gateHeight) / 2;

            for (int x = wallStartingIndex; x < wallStartingIndex + wallThickness; x++)
            {
                for (int y = 0; y < BattleFieldSize.y; y++)
                {
                    bool isGate = ((gateStartingIndex <= y) && (y < gateStartingIndex + gateHeight));
                    var structure = new Structure(isGate, new Vector2Int(x, y), isGate ? gateHealth : wallHealth);
                    MapUnits[x, y] = structure;
                    structure.OnDestroyed += (s) => structures.Remove(s);

                    structures.Add(structure);
                }
            }
        }

        private void CreateUnits(ArmyCommander commander, List<Unit> units, bool isAttacker, int wallThickness)
        {
            var queue = new Queue<Unit>(units);
            int placementDirectionSign = isAttacker ? -1 : 1;
            int startingColumnIndex = (BattleFieldSize.x - wallThickness) / 2 +
                (isAttacker ? -(1 + BattleFieldSettings.AttackersMinDistanceFromWall) : wallThickness);
            int centerIndexY = Mathf.FloorToInt(BattleFieldSize.y / 2.0f);

            for (int x = startingColumnIndex; (0 <= x) && (x < BattleFieldSize.x); x += placementDirectionSign)
            {
                for (int offsetY = 0; IsWithinBounds(x, centerIndexY - offsetY) || IsWithinBounds(x, centerIndexY + offsetY); offsetY++)
                {
                    TryAddUnit(x, centerIndexY + offsetY, queue, commander);

                    if (offsetY > 0)
                    {
                        TryAddUnit(x, centerIndexY - offsetY, queue, commander);
                    }
                    if (units.Count == 0)
                    {
                        return;
                    }
                }
            }
        }

        private void TryAddUnit(int x, int y, Queue<Unit> units, ArmyCommander commander)
        {
            if ((units.Count > 0) && IsWithinBounds(x, y))
            {
                var warrior = new Warrior(units.Dequeue(), commander.IsAttacker, new Vector2Int(x, y));
                MapUnits[x, y] = warrior;
                commander.AddWarrior(warrior);
            }
        }

        private bool IsWithinBounds(int x, int y)
        {
            return (0 <= x) && (x < BattleFieldSize.x) &&
                (0 <= y) && (y < BattleFieldSize.y);
        }
    }
}
