using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GenerateMapScreen : UiUnit<object>
{
    const int N = 3;

    class TileData
    {
        public TileType[,] tiles;
        public int weight;

        public TileData()
        {
            tiles = new TileType[3,3];
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


    [SerializeField] private TileTextureData tileTexture = default;
    [SerializeField] private Image tileViewPrefab = default;
    [SerializeField] private GridLayoutGroup gridLayout = default;
    [SerializeField] private Vector2Int outputMapResolution = default;
    [SerializeField] private Button closeButton = default;

    private List<Image> tiles = new List<Image>();


    private void Awake()
    {
        closeButton.onClick.AddListener(() => Hide(null));    
    }


    public override void Show(Action<object> onHidden = null)
    {
        base.Show(onHidden);

        tiles.ForEach((tile) => Destroy(tile.gameObject));
        tiles.Clear();

        List<TileData> inputTiles = GenerateInputTileset();
        //TileType[,] outputMap = GenerateOutputMap(inputTiles);

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = outputMapResolution.x;

        for (int i = 0; i < outputMapResolution.x * outputMapResolution.y; i++)
        {
            Image tile = Instantiate(tileViewPrefab, gridLayout.transform);
            tile.gameObject.SetActive(true);
            tiles.Add(tile);
        }

        StartCoroutine(GenerateOutputMap(inputTiles));
    }


    private List<TileData> GenerateInputTileset()
    {
        Vector2Int resolution = tileTexture.Resolution;

        List<TileData> inputTileset = new List<TileData>();

        for (int startingTileX = 0; startingTileX < resolution.x - N + 1; startingTileX++)
        {
            for (int startingTileY = 0; startingTileY < resolution.y - N + 1; startingTileY++)
            {
                TileData tileData = new TileData();

                for (int offsetX = 0; offsetX < N; offsetX++)
                {
                    for (int offsetY = 0; offsetY < N; offsetY++)
                    {
                        tileData.tiles[offsetX, offsetY] = tileTexture.GetTileType(GetIndex(startingTileX + offsetX, startingTileY + offsetY));
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

        foreach (var data in inputTileset)
        {
            Debug.Log($"({data.weight}  {data.tiles[0, 0]}:{data.tiles[1, 0]}:{data.tiles[2, 0]}|{data.tiles[0, 1]}:{data.tiles[1, 1]}:{data.tiles[2, 1]}|{data.tiles[0, 2]}:{data.tiles[1, 2]}:{data.tiles[2, 2]})");
        }

        return inputTileset;

        int GetIndex(int x, int y)
        {
            return x + y * resolution.x;
        }
    }


    private IEnumerator GenerateOutputMap(List<TileData> inputTiles)
    {
        List<TileData>[,] entropyMap = new List<TileData>[outputMapResolution.x, outputMapResolution.y];
        TileType[,] outputMap = new TileType[outputMapResolution.x, outputMapResolution.y];

        for (int x = 0; x < entropyMap.GetLength(0); x++)
        {
            for (int y = 0; y < entropyMap.GetLength(1); y++)
            {
                entropyMap[x, y] = new List<TileData>(inputTiles);
            }
        }

        while (true)
        {
            yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));
            Vector2Int lowestEntropyElementIndex = GetLowestEntropyElement(entropyMap);

            //Get random tile
            List<TileData> element = entropyMap[lowestEntropyElementIndex.x, lowestEntropyElementIndex.y];
            TileData tile = GetRandomTile(element);
            //Apply random tile
            ApplyTileData(outputMap, lowestEntropyElementIndex, tile);

            //Recalculate entropy map and remove completed tiles
            RecalculateEntropyMap(entropyMap, outputMap, lowestEntropyElementIndex);
            DrawMap(outputMap);

            bool isFilled = true;

            foreach (var outputTile in outputMap)
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

            if (!isFilled && !hasElements)
            {
                throw new Exception();
            }

            yield return null;
        }
    }


    private Vector2Int GetLowestEntropyElement(List<TileData>[,] entropyMap)
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


    private TileData GetRandomTile(List<TileData> possibleElements)
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


    private void ApplyTileData(TileType[,] outputMap, Vector2Int position, TileData tileData)
    {
        for (int x = 0; (x < N) && (position.x + x < outputMap.GetLength(0)); x++)
        {
            for (int y = 0; (y < N) && (position.y + y < outputMap.GetLength(1)); y++)
            {
                outputMap[position.x + x, position.y + y] = tileData.tiles[x, y];
            }
        }
    }


    private void RecalculateEntropyMap(List<TileData>[,] entropyMap, TileType[,] outputMap, Vector2Int filledTilePosition)
    {
        //We recalculate only the the tiles that could've been affected by tile applying
        Vector2Int topLeftCorner = filledTilePosition - Vector2Int.one * (N - 1);
        topLeftCorner.x = Mathf.Max(0, topLeftCorner.x);
        topLeftCorner.y = Mathf.Max(0, topLeftCorner.y);

        //Iterate over all possibly changed elements
        for (int x = topLeftCorner.x; (x < outputMap.GetLength(0)) && (x < filledTilePosition.x + N); x++)
        {
            for (int y = topLeftCorner.y; (y < outputMap.GetLength(1)) && (y < filledTilePosition.y + N); y++)
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

                                if ((tileType != TileType.None) && (tileType != tileElements[i].tiles[offsetX, offsetY]))
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

    private void DrawMap(TileType[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                tiles[x + y * map.GetLength(0)].color = TilesContainer.GetTileColor(map[x, y]);
            }
        }
    }
}
