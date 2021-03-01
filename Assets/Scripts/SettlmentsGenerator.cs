using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public static class SettlmentsGenerator
{
    public static (Vector2Int[] castles, Vector2Int[] villages) Generate(TileType[,] map)
    {
        Vector2Int[] castles = GetCastlesPositions(map);
        Vector2Int[] villages = GetVillagesPositions(map, castles);

        return (castles, villages);
    }


    private static Vector2Int[] GetCastlesPositions(TileType[,] map)
    {
        Vector2Int[] castles = new Vector2Int[2];
        List<Vector2Int> castleSpawnTiles = new List<Vector2Int>();

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if ((((MapGenerationSettings.MinCastleDistanceFromBorder - 1 <= x) && (x <= MapGenerationSettings.MaxCastleDistanceFromBorder - 1)) ||
                    ((map.GetLength(0) - MapGenerationSettings.MaxCastleDistanceFromBorder <= x) && (x <= map.GetLength(0) - MapGenerationSettings.MinCastleDistanceFromBorder))) &&
                    (((MapGenerationSettings.MinCastleDistanceFromBorder - 1 <= y) && (y <= MapGenerationSettings.MaxCastleDistanceFromBorder - 1)) ||
                    ((map.GetLength(1) - MapGenerationSettings.MaxCastleDistanceFromBorder <= y) && (y <= map.GetLength(1) - MapGenerationSettings.MinCastleDistanceFromBorder))))
                {
                    if (Array.Exists(MapGenerationSettings.PossibleCastleSpawnTiles, (tile) => tile == map[x, y]))
                    {
                        castleSpawnTiles.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        castles[0] = castleSpawnTiles.RandomObject();

        List<(Vector2Int, float)> enemyCastleSpawnTiles = new List<(Vector2Int, float)>();

        foreach (var tile in castleSpawnTiles)
        {
            int distanceFromPlayerCastle = Mathf.Abs(tile.x - castles[0].x) + Mathf.Abs(tile.y - castles[0].y);

            if (MapGenerationSettings.MinDistanceBetweenCastles < distanceFromPlayerCastle)
            {
                enemyCastleSpawnTiles.Add((tile, Mathf.Pow(distanceFromPlayerCastle, MapGenerationSettings.DistanceBetweenCastlesPower)));
            }
        }

        float randomWeight = Random.Range(0, enemyCastleSpawnTiles.Sum((tile) => tile.Item2));

        foreach (var tile in enemyCastleSpawnTiles)
        {
            if (randomWeight < tile.Item2)
            {
                castles[1] = tile.Item1;
                break;
            }

            randomWeight -= tile.Item2;
        }

        return castles;
    }


    private static Vector2Int[] GetVillagesPositions(TileType[,] map, Vector2Int[] castles)
    {
        var villages = new Vector2Int[MapGenerationSettings.SpawnedVillagesCount];

        for (int i = 0; i < MapGenerationSettings.SpawnedVillagesCount; i++)
        {
            List<(Vector2Int, float)> villageSpawnTiles = new List<(Vector2Int, float)>();

            for (int x = MapGenerationSettings.MinVillageDistanceFromBorder; x < map.GetLength(0) - MapGenerationSettings.MinVillageDistanceFromBorder; x++)
            {
                for (int y = MapGenerationSettings.MinVillageDistanceFromBorder; y < map.GetLength(1) - MapGenerationSettings.MinVillageDistanceFromBorder; y++)
                {
                    if (!Array.Exists(MapGenerationSettings.PossibleVillageSpawnTiles, (tile) => tile == map[x, y]))
                    {
                        continue;
                    }

                    int distancesSum = 0;

                    bool isPossible = true;

                    foreach (var castle in castles)
                    {
                        int distanceToCastle = Mathf.Abs(x - castle.x) + Mathf.Abs(y - castle.y);

                        if (MapGenerationSettings.MinDistanceBetweenSettlments < distanceToCastle)
                        {
                            distancesSum += distanceToCastle;
                        }
                        else
                        {
                            isPossible = false;
                        }
                    }

                    foreach (var village in villages)
                    {
                        int distanceToVillage = Mathf.Abs(x - village.x) + Mathf.Abs(y - village.y);

                        if (MapGenerationSettings.MinDistanceBetweenSettlments < distanceToVillage)
                        {
                            distancesSum += distanceToVillage;
                        }
                        else
                        {
                            isPossible = false;
                        }
                    }

                    if (isPossible)
                    {
                        villageSpawnTiles.Add((new Vector2Int(x, y), Mathf.Pow(distancesSum, MapGenerationSettings.DistanceToVillagePower)));
                    }
                }
            }

            float randomWeight = Random.Range(0, villageSpawnTiles.Sum((tile) => tile.Item2));

            foreach (var tile in villageSpawnTiles)
            {
                if (randomWeight < tile.Item2)
                {
                    villages[i] = tile.Item1;
                    break;
                }

                randomWeight -= tile.Item2;
            }
        }

        return villages;
    }
}
