using Domination;
using Domination.Utils;
using System;
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

        private Tile[,] tiles;

        private int characterIndex;

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

                    tile.SetType(tilesTypes[x, y]);
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

            foreach (var villagePosition in settlments.villages)
            {
                Tile correspondingTile = tiles[villagePosition.x, villagePosition.y];
                Village village = Instantiate(villagePrefab, correspondingTile.transform, false);
                correspondingTile.AttachSettlment(village);
            }

            cameraController.SetRestrictions(-halfMapSize, halfMapSize);

            isFirstTurn = true;

            Characters = new Character[2];

            Characters[0] = new Player(castles[0]);
            castles[0].Lord = Characters[0];

            Characters[1] = new AiCharacter(castles[1]);
            castles[1].Lord = Characters[1];

            LevelWrapper wrapper = new LevelWrapper(this);

            BuildingSystem.Init(wrapper);
            RecruitmentSystem.Init(wrapper);

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

            Characters[characterIndex].OnTurnFinish += OnCharacterTurnFinish;
            Characters[characterIndex].StartTurn(isFirstTurn);
        }
    }
}
