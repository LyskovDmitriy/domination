using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public static class WafeFunctionCollapse
{
    private class TileData
    {
        public TileType[,] tiles;
        public int weight;

        public TileData()
        {
            tiles = new TileType[3, 3];
            weight = 1;
        }


        public override bool Equals(object obj)
        {
            if (obj is TileData data)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    for (int y = 0; y < tiles.GetLength(1); y++)
                    {
                        if (tiles[x, y] != data.tiles[x, y])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }
    }


    private const int N = 3;


    public static TileType[,] GenerateMap(Vector2Int outputMapResolution, TileTextureData tileTexture)
    {
        List<TileData> inputTiles = GenerateInputTileset(tileTexture);

        TileType[,] outputMap = new TileType[outputMapResolution.x, outputMapResolution.y];
        TileType[,] calculationsMap = ConvertToMapWithBorders(outputMap);
        List<TileData>[,] entropyMap = GetEntropyMap(inputTiles, calculationsMap, outputMapResolution);

        while (true)
        {
            Vector2Int lowestEntropyElementIndex = GetLowestEntropyElement(entropyMap);

            //Get random tile
            List<TileData> element = entropyMap[lowestEntropyElementIndex.x, lowestEntropyElementIndex.y];
            TileData tile = GetRandomTile(element);
            //Apply random tile
            ApplyTileData(calculationsMap, lowestEntropyElementIndex, tile);

            //Recalculate entropy map and remove completed tiles
            //We recalculate only the the tiles that could've been affected by tile applying
            Vector2Int offset = Vector2Int.one * (N - 1);
            RecalculateEntropyMap(entropyMap, calculationsMap, lowestEntropyElementIndex - offset, lowestEntropyElementIndex + offset);

            bool isFilled = true;
            foreach (var outputTile in calculationsMap)
            {
                if (outputTile == TileType.None)
                {
                    isFilled = false;
                    break;
                }
            }

            if (isFilled)
            {
                break;
            }

            bool hasElements = false;
            foreach (var mapElement in entropyMap)
            {
                if (mapElement != null)
                {
                    hasElements = true;
                }
            }

            if (!hasElements)
            {
                throw new System.Exception();
            }
        }

        for (int x = 0; x < outputMapResolution.x; x++)
        {
            for (int y = 0; y < outputMapResolution.y; y++)
            {
                outputMap[x, y] = calculationsMap[x + N - 1, y + N - 1];
            }
        }

        return outputMap;
    }


    private static List<TileData> GenerateInputTileset(TileTextureData tileTexture)
    {
        TileType[,] tileset = new TileType[tileTexture.Resolution.x, tileTexture.Resolution.y];

        for (int x = 0; x < tileTexture.Resolution.x; x++)
        {
            for (int y = 0; y < tileTexture.Resolution.y; y++)
            {
                tileset[x, y] = tileTexture.GetTileType(x + y * tileTexture.Resolution.x);
            }
        }

        tileset = ConvertToMapWithBorders(tileset);


        List<TileData> inputTileset = new List<TileData>();
        for (int startingTileX = 0; startingTileX < tileset.GetLength(0) - N + 1; startingTileX++)
        {
            for (int startingTileY = 0; startingTileY < tileset.GetLength(1) - N + 1; startingTileY++)
            {
                TileData tileData = new TileData();

                for (int offsetX = 0; offsetX < N; offsetX++)
                {
                    for (int offsetY = 0; offsetY < N; offsetY++)
                    {
                        tileData.tiles[offsetX, offsetY] = tileset[startingTileX + offsetX, startingTileY + offsetY];
                    }
                }

                var matchingData = inputTileset.Find((data) => data.Equals(tileData));

                if (matchingData == null)
                {
                    inputTileset.Add(tileData);
                }
                else
                {
                    matchingData.weight++;
                }
            }
        }

        return inputTileset;
    }


    private static List<TileData>[,] GetEntropyMap(List<TileData> inputTiles, TileType[,] outputMap, Vector2Int outputMapResolution)
    {
        Vector2Int entropyMapResolution = outputMapResolution + Vector2Int.one * (N - 1);
        List<TileData>[,] entropyMap = new List<TileData>[entropyMapResolution.x, entropyMapResolution.y];

        for (int x = 0; x < entropyMapResolution.x; x++)
        {
            for (int y = 0; y < entropyMapResolution.y; y++)
            {
                entropyMap[x, y] = new List<TileData>(inputTiles);
            }
        }

        RecalculateEntropyMap(entropyMap, outputMap, Vector2Int.zero, entropyMapResolution - Vector2Int.one);

        return entropyMap;
    }


    private static void RecalculateEntropyMap(List<TileData>[,] entropyMap, TileType[,] outputMap, Vector2Int topLeftCorner, Vector2Int topRightCorner)
    {
        topLeftCorner.x = Mathf.Max(0, topLeftCorner.x);
        topLeftCorner.y = Mathf.Max(0, topLeftCorner.y);

        //Iterate over all possibly changed elements
        for (int x = Mathf.Max(0, topLeftCorner.x); (x < entropyMap.GetLength(0)) && (x <= topRightCorner.x); x++)
        {
            for (int y = Mathf.Max(0, topLeftCorner.y); (y < entropyMap.GetLength(1)) && (y <= topRightCorner.y); y++)
            {
                List<TileData> tileElements = entropyMap[x, y];

                if (tileElements != null)
                {
                    bool isFilled = true;
                    //Null element if it is filled
                    for (int offsetX = 0; (offsetX < N) && (x + offsetX < outputMap.GetLength(0)); offsetX++)
                    {
                        for (int offsetY = 0; (offsetY < N) && (y + offsetY < outputMap.GetLength(1)); offsetY++)
                        {
                            if (outputMap[x + offsetX, y + offsetY] == TileType.None)
                            {
                                isFilled = false;
                                break;
                            }
                        }

                        if (!isFilled)
                        {
                            break;
                        }
                    }

                    if (isFilled)
                    {
                        entropyMap[x, y] = null;
                        continue;
                    }

                    //Iterate over all possible tiles for an element
                    for (int i = tileElements.Count - 1; i >= 0; i--)
                    {
                        bool isPossible = true;
                        //Iterate over all tile squares
                        for (int offsetX = 0; (offsetX < N) && (x + offsetX < outputMap.GetLength(0)); offsetX++)
                        {
                            for (int offsetY = 0; (offsetY < N) && (y + offsetY < outputMap.GetLength(1)); offsetY++)
                            {
                                TileType tileType = outputMap[x + offsetX, y + offsetY];

                                if ((tileType != TileType.None) && (tileType != tileElements[i].tiles[offsetX, offsetY]) ||
                                    ((tileType == TileType.None) && (tileElements[i].tiles[offsetX, offsetY] == TileType.Border))) //You can't place new border tiles
                                {
                                    isPossible = false;
                                    break;
                                }
                            }

                            if (!isPossible)
                            {
                                break;
                            }
                        }

                        if (!isPossible)
                        {
                            tileElements.RemoveAt(i);
                        }
                    }

                    if (entropyMap[x, y].Count == 0)
                    {
                        entropyMap[x, y] = null;
                    }
                }
            }
        }
    }


    private static Vector2Int GetLowestEntropyElement(List<TileData>[,] entropyMap)
    {
        List<Vector2Int> lowestEntropyElements = new List<Vector2Int>();
        float lowestEntropy = float.MaxValue;

        for (int x = 0; x < entropyMap.GetLength(0); x++)
        {
            for (int y = 0; y < entropyMap.GetLength(1); y++)
            {
                List<TileData> element = entropyMap[x, y];

                if (element != null)
                {
                    int weightsSum = element.Sum((data) => data.weight);

                    float entropy = 0.0f;


                    foreach (var tile in element)
                    {
                        float probability = ((float)weightsSum) / tile.weight;
                        entropy += probability * Mathf.Log(probability, 2.0f);
                    }

                    if (Mathf.Approximately(lowestEntropy, entropy))
                    {
                        lowestEntropyElements.Add(new Vector2Int(x, y));
                    }
                    else if (entropy < lowestEntropy)
                    {
                        lowestEntropy = entropy;
                        lowestEntropyElements.Clear();
                        lowestEntropyElements.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        return lowestEntropyElements[Random.Range(0, lowestEntropyElements.Count)];
    }


    private static TileData GetRandomTile(List<TileData> possibleElements)
    {
        int totalWeight = possibleElements.Sum((data) => data.weight);
        int randomValue = Random.Range(0, totalWeight);

        foreach (var tile in possibleElements)
        {
            if (randomValue < tile.weight)
            {
                return tile;
            }

            randomValue -= tile.weight;
        }

        return null;
    }


    private static void ApplyTileData(TileType[,] outputMap, Vector2Int position, TileData tileData)
    {
        for (int x = 0; (x < N) && (position.x + x < outputMap.GetLength(0)); x++)
        {
            for (int y = 0; (y < N) && (position.y + y < outputMap.GetLength(1)); y++)
            {
                outputMap[position.x + x, position.y + y] = tileData.tiles[x, y];
            }
        }
    }



    private static TileType[,] ConvertToMapWithBorders(TileType[,] map)
    {
        Vector2Int resultSize = new Vector2Int(map.GetLength(0) + N * 2 - 2, map.GetLength(1) + N * 2 - 2);
        TileType[,] output = new TileType[resultSize.x, resultSize.y];

        for (int x = 0; x < resultSize.x; x++)
        {
            for (int y = 0; y < resultSize.y; y++)
            {
                if ((x < N - 1) || (x > resultSize.x - N) ||
                    (y < N - 1) || (y > resultSize.y - N))
                {
                    output[x, y] = TileType.Border;
                }
                else
                {
                    output[x, y] = map[x - N + 1, y - N + 1];
                }
            }
        }

        return output;
    }
}
