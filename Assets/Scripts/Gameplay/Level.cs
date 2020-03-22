using System.Collections.Generic;
using UnityEngine;


public class Level : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab = default;
    [SerializeField] private Castle castlePrefab = default;
    [SerializeField] private Village villagePrefab = default;
    [SerializeField] private Vector2 distanceBetweenTiles = default;
    [SerializeField] private CameraController cameraController = default;
    [SerializeField] private Selector selector = default;

    private Tile[,] tiles;

    private Character[] characters;
    private int characterIndex;

    bool isFirstTurn;

    public Character Player => characters[0];


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

        characters = new Character[2];

        characters[0] = new Player();
        characters[0].Init(castles[0]);

        characters[1] = new AiCharacter();
        characters[1].Init(castles[1]);

        characters[0].OnTurnFinish += OnCharacterTurnFinish;
        characters[0].StartTurn(true);
    }


    private void OnCharacterTurnFinish()
    {
        characters[characterIndex].OnTurnFinish -= OnCharacterTurnFinish;
        characterIndex++;

        if (characterIndex >= characters.Length)
        {
            isFirstTurn = false;
            characterIndex %= characters.Length;
        }

        characters[characterIndex].OnTurnFinish += OnCharacterTurnFinish;
        characters[characterIndex].StartTurn(isFirstTurn);
    }
}
