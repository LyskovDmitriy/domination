using Domination.LevelLogic;
using Domination.Warfare;
using Domination.Battle.Logic; 
using UnityEngine; 


namespace Domination.Battle.View
{
    public class BattlefieldView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer background = default;
        [Space]
        [SerializeField] private Vector2 tileSize = default;
        [SerializeField] private Vector2 distanceBetweenTiles = default;
        [SerializeField] private GameObject battleFieldTilePrefab = default;
        [Space]
        [SerializeField] private GameObject wallTilePrefab = default;
        [SerializeField] private GameObject gateTilePrefab = default;
        [SerializeField] private WarriorView knightViewPrefab = default;
        [SerializeField] private WarriorView archerViewPrefab = default;

        private BattleController battlefieldController;


        public void Init(Army attackingArmy, Army defendingArmy, Settlment attackedSettlment, Tile settlmentTile)
        {
            battlefieldController = new BattleController(attackingArmy, defendingArmy, attackedSettlment, settlmentTile);

            background.color = TilesContainer.GetTileColor(settlmentTile.Type);

            CreateTiles();
        }

        private void CreateTiles()
        {
            int fieldSizeX = battlefieldController.BattleFieldSize.x;
            int fieldSizeY = battlefieldController.BattleFieldSize.y;
            for (int x = 0; x < fieldSizeX; x++)
            {
                for (int y = 0; y < fieldSizeY; y++)
                {
                    var tilePosition = GetTilePosition(x, y);
                    Instantiate(battleFieldTilePrefab, tilePosition, Quaternion.identity, transform);

                    var mapUnit = battlefieldController.MapUnits[x, y];

                    if (mapUnit != null)
                    {
                        switch (mapUnit)
                        {
                            case Structure structure:

                                CreateMapUnit(structure.isGate ? gateTilePrefab : wallTilePrefab, tilePosition);

                                break;

                            case Warrior warrior:

                                var warriorView = CreateMapUnit(
                                    (warrior.Unit.WeaponType == WeaponType.Melee) ? knightViewPrefab : archerViewPrefab, 
                                    tilePosition);
                                warriorView.Init((fieldSizeX - 1) / 2.0f < x);

                                break;
                        }
                    }
                }
            }
        }

        private Vector2 GetTilePosition(int x, int y)
        {
            int fieldSizeX = battlefieldController.BattleFieldSize.x;
            int fieldSizeY = battlefieldController.BattleFieldSize.y;

            Vector2 tilePosition =
                tileSize / 2 - //initial tile anchor offset
                new Vector2(tileSize.x * fieldSizeX / 2 + distanceBetweenTiles.x * (fieldSizeX - 1) / 2,
                    tileSize.y * fieldSizeY / 2 + distanceBetweenTiles.y * (fieldSizeY - 1) / 2) +
                new Vector2(
                    x * (tileSize.x + distanceBetweenTiles.x),
                    y * (tileSize.y + distanceBetweenTiles.y)); //distance to next tile

            return tilePosition;
        }

        private T CreateMapUnit<T>(T prefab, Vector2 position) where T : Object =>
            Instantiate(prefab, position, Quaternion.identity, transform);
    }
}
