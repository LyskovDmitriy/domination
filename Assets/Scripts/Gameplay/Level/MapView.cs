using Domination.Data;
using Domination.EventsSystem;
using System.Collections.Generic;
using UnityEngine;


namespace Domination.LevelView
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private TileView tilePrefab = default;
        [SerializeField] private SettlmentView castlePrefab = default;
        [SerializeField] private SettlmentView villagePrefab = default;
        [SerializeField] private Vector2 distanceBetweenTiles = default;
        [SerializeField] private CameraController cameraController = default;
        [SerializeField] private Selector selector = default;
        [SerializeField] private FogSystem fogSystem = default;

        private TileView[,] tiles;

        private List<SettlmentView> settlments = new List<SettlmentView>();

        public static MapView Instance { get; private set; }

        public Level Level { get; private set; }
        public BuildingSystem BuildingSystem => Level.BuildingSystem;
        public RecruitmentSystem RecruitmentSystem => Level.RecruitmentSystem;

        public Character Player => Level.Player;


        private void Awake()
        {
            Instance = this;
            selector.Init(cameraController.Camera);
        }

        private void OnDestroy() => Instance = null;

        public void Create(Level level)
        {
            Level = level;
            InitView();
        }

        public SettlmentView FindSettlment(Vector2Int position) => settlments.Find(s => s.Settlment.Position == position);

        public LevelData GetData() => Level.GetData();

        private void InitView()
        {
            Level.OnTurnFinished += OnTurnFinished;

            int sizeX = Level.Map.GetLength(0);
            int sizeY = Level.Map.GetLength(1);

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

                    tile.Init(new Vector2Int(x, y), Level.Map[x, y]);
                    tiles[x, y] = tile;
                }
            }

            foreach (var castle in Level.Castles)
            {
                TileView correspondingTile = tiles[castle.Position.x, castle.Position.y];
                SettlmentView castleView = Instantiate(castlePrefab, correspondingTile.transform, false);
                castleView.Init(castle);
                correspondingTile.AttachSettlment(castleView);
                settlments.Add(castleView);
            }

            foreach (var village in Level.Villages)
            {
                TileView correspondingTile = tiles[village.Position.x, village.Position.y];
                SettlmentView villageView = Instantiate(villagePrefab, correspondingTile.transform, false);
                villageView.Init(village);
                correspondingTile.AttachSettlment(villageView);
                settlments.Add(villageView);
            }

            SettlmentView playerCastleView = FindSettlment(Player.Castle.Position);
            cameraController.Init(playerCastleView.transform.position, -halfMapSize, halfMapSize);

            fogSystem.Init(tiles);
            fogSystem.ApplyFog(Level.CurrentTurn, Player);
        }

        private void OnTurnFinished()
        {
            if (Level.ActiveCharacter == Player)
            {
                fogSystem.ApplyFog(Level.CurrentTurn, Player);
            }
        }
    }
}
