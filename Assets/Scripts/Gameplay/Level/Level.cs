using Domination.Generator;
using Domination.LevelView;
using Domination.Utils;
using System.Collections.Generic;
using UnityEngine;
using Utils;


namespace Domination
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private TileView tilePrefab = default;
        [SerializeField] private SettlmentView castlePrefab = default;
        [SerializeField] private SettlmentView villagePrefab = default;
        [SerializeField] private Vector2 distanceBetweenTiles = default;
        [SerializeField] private CameraController cameraController = default;
        [SerializeField] private Selector selector = default;
        [SerializeField] private FogSystem fogSystem = default;


        private LevelMap levelMap;
        private TileView[,] tiles;
        private List<SettlmentView> settlments = new List<SettlmentView>();

        private int characterIndex;
        private int currentTurnIndex;

        private bool isFirstTurn;

        private Character neutralCharacter;


        public Character Player => Characters[0];
        public Character[] Characters { get; private set; }
        //public TileType[,] Map { get; private set; }


        private void Awake()
        {
            selector.Init(cameraController.Camera);
        }

        public void Create()
        {
            levelMap = LevelGenerator.Generate();
            int sizeX = levelMap.map.GetLength(0);
            int sizeY = levelMap.map.GetLength(1);

            Vector2 halfMapSize = new Vector3(distanceBetweenTiles.x * sizeX, distanceBetweenTiles.y * sizeY) / 2.0f;
            tiles = new TileView[sizeX, sizeY];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    TileView tile = Instantiate(tilePrefab,
                        new Vector2(distanceBetweenTiles.x * x, distanceBetweenTiles.y * y) + distanceBetweenTiles / 2.0f - halfMapSize,
                        Quaternion.identity,
                        transform);

                    tile.Init(new Vector2Int(x, y), levelMap.map[x, y]);
                    tiles[x, y] = tile;
                }
            }

            foreach (var castle in levelMap.castles)
            {
                TileView correspondingTile = tiles[castle.Position.x, castle.Position.y];
                SettlmentView castleView = Instantiate(castlePrefab, correspondingTile.transform, false);
                castleView.Init(castle);
                correspondingTile.AttachSettlment(castleView);
                settlments.Add(castleView);
            }

            foreach (var village in levelMap.villages)
            {
                TileView correspondingTile = tiles[village.Position.x, village.Position.y];
                SettlmentView villageView = Instantiate(villagePrefab, correspondingTile.transform, false);
                villageView.Init(village);
                correspondingTile.AttachSettlment(villageView);
                settlments.Add(villageView);
            }

            isFirstTurn = true;

            Characters = new Character[2];

            Characters[0] = new Player();
            Characters[0].AddSettlment(levelMap.castles[0]);

            Characters[1] = new AiCharacter();
            Characters[1].AddSettlment(levelMap.castles[1]);

            SettlmentView playerCastleView = FindSettlment(Player.Castle.Position);
            cameraController.Init(playerCastleView.transform.position, -halfMapSize, halfMapSize);

            LevelWrapper wrapper = new LevelWrapper(this);

            BuildingSystem.Init(wrapper);
            RecruitmentSystem.Init(wrapper);

            neutralCharacter = new Character();

            foreach (var village in levelMap.villages)
            {
                neutralCharacter.AddSettlment(village);
                RecruitmentSystem.SetupNeutralVillageArmy(village);
            }

            fogSystem.Init(tiles);
            fogSystem.ApplyFog(currentTurnIndex, Player);

            Characters[0].OnTurnFinish += OnCharacterTurnFinish;
            Characters[0].StartTurn(true);
        }

        public float CalculateDistanceBetweenSettlments(Settlment startingSettlment, Settlment targetSettlment) => 
            Pathfinding.GetDistance(startingSettlment.Position, targetSettlment.Position, levelMap.simpleMap, TilesPassingCostContainer.GetTilePassingCost);

        public SettlmentView FindSettlment(Vector2Int position) => settlments.Find(s => s.Settlment.Position == position);

        private void OnCharacterTurnFinish()
        {
            Characters[characterIndex].OnTurnFinish -= OnCharacterTurnFinish;
            characterIndex++;

            if (characterIndex >= Characters.Length)
            {
                isFirstTurn = false;
                characterIndex %= Characters.Length;
            }

            if (characterIndex == 0)
            {
                currentTurnIndex++;
            }

            if (Characters[characterIndex] == Player)
            {
                fogSystem.ApplyFog(currentTurnIndex, Player);
            }

            Characters[characterIndex].OnTurnFinish += OnCharacterTurnFinish;
            Characters[characterIndex].StartTurn(isFirstTurn);
        }
    }
}
