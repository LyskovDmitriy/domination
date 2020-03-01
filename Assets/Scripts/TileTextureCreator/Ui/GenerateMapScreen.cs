using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GenerateMapScreen : UiUnit<object>
{
    [SerializeField] private Image tileViewPrefab = default;
    [SerializeField] private GridLayoutGroup gridLayout = default;
    [SerializeField] private Button closeButton = default;
    [SerializeField] private Sprite castleSprite = default;
    [SerializeField] private Sprite villageSprite = default;

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

        TileType[,] outputMap = WafeFunctionCollapse.GenerateMap(MapGenerationSettings.MapResolution, MapGenerationSettings.TileTexture);

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = outputMap.GetLength(0);

        for (int i = 0; i < outputMap.GetLength(0) * outputMap.GetLength(1); i++)
        {
            Image tile = Instantiate(tileViewPrefab, gridLayout.transform);
            tile.gameObject.SetActive(true);
            tiles.Add(tile);
        }

        var buildings = SettlmentsGenerator.Generate(outputMap);

        DrawMap(outputMap, buildings.Item1, buildings.Item2);
    }


    private void DrawMap(TileType[,] map, Vector2Int[] castles, List<Vector2Int> villages)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                tiles[x + y * map.GetLength(0)].color = TilesContainer.GetTileColor(map[x, y]);
            }
        }

        foreach (var catlePosition in castles)
        {
            Image castle = Instantiate(tileViewPrefab, tiles[catlePosition.x + catlePosition.y * map.GetLength(0)].transform);
            castle.rectTransform.anchoredPosition = Vector2.zero;
            castle.sprite = castleSprite;
            castle.gameObject.SetActive(true);
        }

        foreach (var villagePosition in villages)
        {
            Image village = Instantiate(tileViewPrefab, tiles[villagePosition.x + villagePosition.y * map.GetLength(0)].transform);
            village.rectTransform.anchoredPosition = Vector2.zero;
            village.sprite = villageSprite;
            village.gameObject.SetActive(true);
        }
    }
}
