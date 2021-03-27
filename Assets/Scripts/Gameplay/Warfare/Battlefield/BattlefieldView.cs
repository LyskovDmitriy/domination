using Domination.LevelLogic;
using Domination.Warfare;
using Domination.Battle.Logic; 
using UnityEngine; 


namespace Domination.Battle.View
{
    public class BattlefieldView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer background = default;
        [SerializeField] private GameObject battleFieldTile = default;
        [SerializeField] private Vector2 tileSize = default;
        [SerializeField] private Vector2 distanceBetweenTiles = default;

        private BattleController battlefieldController;


        public void Init(Level level, uint attackedSettlmentId)
        {
            var attackedSettlment = level.GetSettlment(attackedSettlmentId);
            
            var attackingArmy = level.Player.GetSettlmentArmy(attackedSettlment);
            var defendingArmy = attackedSettlment.Lord.GetSettlmentArmy(attackedSettlment);

            var settlmentTile = level.Map[attackedSettlment.Position.x, attackedSettlment.Position.y];

            battlefieldController = new BattleController(attackingArmy, defendingArmy, attackedSettlment, settlmentTile);

            background.color = TilesContainer.GetTileColor(settlmentTile.Type);

            int fieldSizeX = battlefieldController.BattleFieldSize.x;
            int fieldSizeY = battlefieldController.BattleFieldSize.y;
            for (int x = 0; x < fieldSizeX; x++)
            {
                for (int y = 0; y < fieldSizeY; y++)
                {
                    Vector2 tilePostion =
                        tileSize / 2 - //initial tile anchor offset
                        new Vector2(tileSize.x * fieldSizeX / 2 + distanceBetweenTiles.x * (fieldSizeX -1) / 2, 
                            tileSize.y * fieldSizeY / 2 + distanceBetweenTiles.y * (fieldSizeY - 1) / 2) +
                        new Vector2(
                            x * (tileSize.x + distanceBetweenTiles.x), 
                            y * (tileSize.y + distanceBetweenTiles.y)); //distance to next tile
                    Instantiate(battleFieldTile, tilePostion, Quaternion.identity, transform);
                }
            }
        }
    }
}
