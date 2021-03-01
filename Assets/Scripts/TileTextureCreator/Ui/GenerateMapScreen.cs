using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils.Ui;


namespace Generator.Ui
{
    public class GenerateMapScreen : UiUnit
    {
        [SerializeField] private Image tileViewPrefab = default;
        [SerializeField] private GridLayoutGroup gridLayout = default;
        [SerializeField] private Button closeButton = default;
        [SerializeField] private Sprite castleSprite = default;
        [SerializeField] private Sprite villageSprite = default;

        private List<Image> tiles = new List<Image>();

        private Action onHidden;


        private void Awake()
        {
            closeButton.onClick.AddListener(Hide);
        }


        public void Show(Action onHidden)
        {
            base.Show();

            tiles.ForEach((tile) => Destroy(tile.gameObject));
            tiles.Clear();

            TileType[,] outputMap = WaveFunctionCollapse.GenerateMap(MapGenerationSettings.MapResolution, MapGenerationSettings.TileTexture);

            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = outputMap.GetLength(0);

            for (int i = 0; i < outputMap.GetLength(0) * outputMap.GetLength(1); i++)
            {
                Image tile = Instantiate(tileViewPrefab, gridLayout.transform);
                tile.gameObject.SetActive(true);
                tiles.Add(tile);
            }

            var settlments = SettlmentsGenerator.Generate(outputMap);

            DrawMap(outputMap, settlments.castles, settlments.villages);

            this.onHidden = onHidden;
        }


        public override void Hide()
        {
            base.Hide();

            onHidden?.Invoke();
            onHidden = null;
        }


        private void DrawMap(TileType[,] map, Vector2Int[] castles, Vector2Int[] villages)
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
}
