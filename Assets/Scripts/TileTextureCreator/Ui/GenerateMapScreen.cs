using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GenerateMapScreen : UiUnit<object>
{
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

        TileType[,] outputMap = WafeFunctionCollapse.GenerateMap(outputMapResolution, tileTexture);

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = outputMap.GetLength(0);

        for (int i = 0; i < outputMap.GetLength(0) * outputMap.GetLength(1); i++)
        {
            Image tile = Instantiate(tileViewPrefab, gridLayout.transform);
            tile.gameObject.SetActive(true);
            tiles.Add(tile);
        }

        DrawMap(outputMap);
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
