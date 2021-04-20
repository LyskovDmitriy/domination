using UnityEngine;
using Domination.Battle.View;
using Domination.Battle.Logic;
using Domination.Battle.Settings;
using Domination.Battle.Logic.Ai;

namespace Domination.Battle.Test
{
    public class BattleFieldDebugger : MonoBehaviour
    {
        [SerializeField] private DebugTile debugTilePrefab = default;

        private DebugTile[,] tiles;
        private BattlefieldView battlefieldView;


        public bool IsActive { get; private set; }


        public void Init(BattlefieldView battlefieldView)
        {
            this.battlefieldView = battlefieldView;
            var sizeX = battlefieldView.BattlefieldController.BattleFieldSize.x;
            var sizeY = battlefieldView.BattlefieldController.BattleFieldSize.y;

            tiles = new DebugTile[sizeX, sizeY];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    var tile = Instantiate(debugTilePrefab, battlefieldView.GetTilePosition(x, y), Quaternion.identity, transform);
                    tile.gameObject.SetActive(IsActive);
                    tile.Init(new Vector2Int(x, y));
                    tiles[x, y] = tile;
                }
            }
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;

            foreach (var tile in tiles)
            {
                tile.gameObject.SetActive(IsActive);

                if (!isActive)
                {
                    tile.SetLabelsActive(false);
                }
            }
        }

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                TestPathfinding();
            }
        }

        private void TestPathfinding()
        {
            var collider = Physics2D.OverlapPoint(battlefieldView.BattleCamera.ScreenToWorldPoint(Input.mousePosition));

            if (collider == null)
            {
                return;
            }

            var tile = collider.GetComponent<DebugTile>();

            if (tile == null)
            {
                return;
            }

            var map = battlefieldView.BattlefieldController.MapUnits;

            if (!(map[tile.Position.x, tile.Position.y] is Warrior warrior))
            {
                return;
            }

            var pathfindingData = BattlePathfiner.GetPathfindingData(
                tile.Position,
                battlefieldView.BattlefieldController.MapUnits,
                mapUnit => MapUnitsPassingCost.GetCost(mapUnit, warrior.IsAttacker),
                mapUnit => mapUnit.IsAttacker != warrior.IsAttacker);

            var nodes = pathfindingData.nodes;

            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int y = 0; y < nodes.GetLength(1); y++)
                {
                    var node = nodes[x, y];
                    tiles[x, y].SetDebugInfo(node.distance, node.isPathObstructedByStructure, node.isPathObstructedByWarrior);
                }
            }

            switch (warrior.planner.PlannedAction)
            {
                case MoveAction moveAction:
                    tiles[moveAction.TargetPosition.x, moveAction.TargetPosition.y].SetMovementTarget(true);
                    break;
            }

            if (warrior.planner.CurrentTarget != null)
            {
                var targetPosition = warrior.planner.CurrentTarget.Position;
                tiles[targetPosition.x, targetPosition.y].SetAttackMarkerActive(true);
            }
        }
    }
}
