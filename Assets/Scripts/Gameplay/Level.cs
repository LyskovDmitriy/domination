using Domination.Utils;
using System.Collections.Generic;
using UnityEngine;


namespace Domination
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Tile tilePrefab = default;
        [SerializeField] private Castle castlePrefab = default;
        [SerializeField] private Village villagePrefab = default;
        [SerializeField] private Vector2 distanceBetweenTiles = default;
        [SerializeField] private CameraController cameraController = default;
        [SerializeField] private Selector selector = default;
        [SerializeField] private FogSystem fogSystem = default;

        private Tile[,] tiles;

        private int characterIndex;
        private int currentTurnIndex;

        bool isFirstTurn;

        public Character Player => Characters[0];
        public Character[] Characters { get; private set; }


        private void Awake()
        {
            selector.Init(cameraController.Camera);
        }


        public void Create()
        {
            Vector2Int resolution = MapGenerationSettings.MapResolution;
            Vector2 halfMapSize = new Vector3(distanceBetweenTiles.x * resolution.x, distanceBetweenTiles.y * resolution.y) / 2.0f;
            TileType[,] tilesTypes = WafeFunctionCollapse.GenerateMap(resolution, MapGenerationSettings.TileTexture);

            tiles = new Tile[resolution.x, resolution.y];

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    Tile tile = Instantiate(tilePrefab,
                        new Vector2(distanceBetweenTiles.x * x, distanceBetweenTiles.y * y) + distanceBetweenTiles / 2.0f - halfMapSize,
                        Quaternion.identity,
                        transform);

                    tile.Init(new Vector2Int(x, y), tilesTypes[x, y]);
                    tiles[x, y] = tile;
                }
            }

            var settlments = SettlmentsGenerator.Generate(tilesTypes);

            List<Castle> castles = new List<Castle>();

            foreach (var castlePosition in settlments.castles)
            {
                Tile correspondingTile = tiles[castlePosition.x, castlePosition.y];
                Castle castle = Instantiate(castlePrefab, correspondingTile.transform, false);
                correspondingTile.AttachSettlment(castle);
                castles.Add(castle);
            }

            List<Village> villages = new List<Village>();

            foreach (var villagePosition in settlments.villages)
            {
                Tile correspondingTile = tiles[villagePosition.x, villagePosition.y];
                Village village = Instantiate(villagePrefab, correspondingTile.transform, false);
                correspondingTile.AttachSettlment(village);
                villages.Add(village);
            }

            isFirstTurn = true;

            Characters = new Character[2];

            Characters[0] = new Player(castles[0]);
            castles[0].Lord = Characters[0];

            Characters[1] = new AiCharacter(castles[1]);
            castles[1].Lord = Characters[1];

            cameraController.Init(Player.Castle.transform.position, -halfMapSize, halfMapSize);

            LevelWrapper wrapper = new LevelWrapper(this);

            BuildingSystem.Init(wrapper);
            RecruitmentSystem.Init(wrapper);

            foreach (var village in villages)
            {
                RecruitmentSystem.SetupNeutralVillageArmy(village);
            }

            fogSystem.Init(tiles, Characters);
            fogSystem.ApplyFog(currentTurnIndex, Player);

            Characters[0].OnTurnFinish += OnCharacterTurnFinish;
            Characters[0].StartTurn(true);
        }

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
