using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab = default;
    [SerializeField] private Vector2 distanceBetweenTiles = default;
    [SerializeField] private CameraController cameraController = default;

    private Tile[,] tiles;


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

        
        cameraController.SetRestrictions(-halfMapSize, halfMapSize);
    }
}
