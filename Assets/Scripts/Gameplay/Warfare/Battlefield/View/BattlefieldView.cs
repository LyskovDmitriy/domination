using Domination.LevelLogic;
using Domination.Warfare;
using Domination.Battle.Logic; 
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Domination.Battle.View
{
    public class BattlefieldView : MonoBehaviour
    {
        [SerializeField] private Camera battleCamera = default;
        [SerializeField] private float cameraAdditionalSizeY = default;
        [Space]
        [SerializeField] private SpriteRenderer background = default;
        [Space]
        [SerializeField] private Vector2 tileSize = default;
        [SerializeField] private Vector2 distanceBetweenTiles = default;
        [SerializeField] private GameObject battleFieldTilePrefab = default;
        [Space]
        [SerializeField] private float secondsToStartAttack = default;

        private List<WarriorView> warriors = new List<WarriorView>();


        public Camera BattleCamera => battleCamera;
        public BattleController BattlefieldController { get; private set; }


        public void Init(Army attackingArmy, Army defendingArmy, Settlment attackedSettlment, Tile settlmentTile)
        {
            BattlefieldController = new BattleController(attackingArmy, defendingArmy, attackedSettlment, settlmentTile);
            battleCamera.orthographicSize = 
                (BattlefieldController.BattleFieldSize.y * tileSize.y + (BattlefieldController.BattleFieldSize.y - 1) * distanceBetweenTiles.y) / 2 + 
                cameraAdditionalSizeY;

            background.color = TilesContainer.GetTileColor(settlmentTile.Type);

            CreateMapUnits();
        }

        public async void StartAttack()
        {
            await Task.Delay(Mathf.RoundToInt(secondsToStartAttack * 1000));
            BattleCycle();
        }

        public Vector3 GetTilePosition(int x, int y)
        {
            int fieldSizeX = BattlefieldController.BattleFieldSize.x;
            int fieldSizeY = BattlefieldController.BattleFieldSize.y;

            Vector3 tilePosition =
                tileSize / 2 - //initial tile anchor offset
                new Vector2(tileSize.x * fieldSizeX / 2 + distanceBetweenTiles.x * (fieldSizeX - 1) / 2,
                    tileSize.y * fieldSizeY / 2 + distanceBetweenTiles.y * (fieldSizeY - 1) / 2)
                +
                new Vector2(
                    x * (tileSize.x + distanceBetweenTiles.x),
                    y * (tileSize.y + distanceBetweenTiles.y)); //distance to next tile

            tilePosition = tilePosition.SetZ(tilePosition.y + tilePosition.x / 1000); //To improve sorting

            return tilePosition;
        }

        private Vector3 GetTilePosition(Vector2Int position) => GetTilePosition(position.x, position.y);

        private async void BattleCycle()
        {
            while (true)
            {
                BattlefieldController.PlanTurn();

                //while (!Input.GetKeyDown(KeyCode.Space))
                {
                    await Task.Yield();
                }

                await ExecutePlanVisually();
                var result = BattlefieldController.ExecutePlan();

                if (result != null)
                {
                    Debug.LogError(result.wasSiegeSuccessful ? "Attacker won" : "Defender won");
                    return;
                }
            }
        }

        private void CreateMapUnits()
        {
            int fieldSizeX = BattlefieldController.BattleFieldSize.x;
            int fieldSizeY = BattlefieldController.BattleFieldSize.y;

            for (int x = 0; x < fieldSizeX; x++)
            {
                for (int y = 0; y < fieldSizeY; y++)
                {
                    var tilePosition = GetTilePosition(x, y);
                    Instantiate(battleFieldTilePrefab, tilePosition.ToVector2(), Quaternion.identity, transform);

                    var mapUnit = BattlefieldController.MapUnits[x, y];

                    if (mapUnit != null)
                    {
                        switch (mapUnit)
                        {
                            case Structure structure:
                                MapUnitsCreator.CreateStructure(structure, tilePosition, transform);
                                break;

                            case Warrior warrior:
                                var warriorView = MapUnitsCreator.CreateWarrior(warrior, tilePosition, transform);
                                warriors.Add(warriorView);
                                warriorView.OnDied += w => warriors.Remove(w);
                                break;
                        }
                    }
                }
            }
        }

        private async Task ExecutePlanVisually()
        {
            var planExecutions = new HashSet<Task>();

            foreach (var warrior in warriors)
            {
                 planExecutions.Add(warrior.ExecutePlan(GetTilePosition));
            }

            await Task.WhenAll(planExecutions);
        }
    }
}
