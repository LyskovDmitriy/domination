using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TileTextureScreen : UiUnit<object>
{
    [SerializeField] private TileView tileViewPrefab = default;
    [SerializeField] private GridLayoutGroup gridLayout = default;
    [SerializeField] private TMP_Dropdown tilesDropdown = default;
    [SerializeField] private CreateTileTextureScreen createTileTextureScreen = default;

    [SerializeField] private Button closeButton = default;
    [SerializeField] private Button saveButton = default;
    [SerializeField] private Button deleteButton = default;
    [SerializeField] private Button createButton = default;

    [SerializeField] private TileView tileVariationButton = default;
    [SerializeField] private RectTransform tileVariationsRoot = default;

    private List<TileView> tiles = new List<TileView>();
    private TileTextureData textureData;

    private TileType selectedTileType;


    private void Awake()
    {
        closeButton.onClick.AddListener(() => Hide(null));
        saveButton.onClick.AddListener(Save);
        deleteButton.onClick.AddListener(() =>
        {
            if (textureData != null)
            {
                TileTexturesHolder.Delete(textureData);
                textureData = null;

                tilesDropdown.value = -1;
                tilesDropdown.RefreshShownValue();
            }
        });
        createButton.onClick.AddListener(Create);

        tilesDropdown.onValueChanged.AddListener((index) => Load(TileTexturesHolder.Textures[index]));

        UpdateTilesDropdown();

        tilesDropdown.value = -1;
        tilesDropdown.RefreshShownValue();

        foreach (var type in Enum.GetValues(typeof(TileType)))
        {
            TileView tileVariation = Instantiate(tileVariationButton, tileVariationsRoot);
            tileVariation.gameObject.SetActive(true);
            tileVariation.TileType = (TileType)type;
            tileVariation.OnClick += (tileView) => selectedTileType = tileView.TileType;
        }
    }


    public override void Show(Action<object> onHidden = null)
    {
        base.Show(onHidden);

        selectedTileType = TileType.None;
    }


    private void Save()
    {
        if (textureData != null)
        {
            textureData.SetData(tiles.Select((tile) => tile.TileType).ToArray());
        }
    }


    private void UpdateTilesDropdown()
    {
        tilesDropdown.ClearOptions();
        tilesDropdown.AddOptions(TileTexturesHolder.Textures.Select((texture) => texture.name).ToList());
    }


    private void Load(TileTextureData textureData)
    {
        this.textureData = textureData;

        tiles.ForEach((tile) => Destroy(tile.gameObject));
        tiles.Clear();

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = textureData.Resolution.x;

        for (int i = 0; i < textureData.Resolution.x * textureData.Resolution.y; i++)
        {
            TileView tile = Instantiate(tileViewPrefab, gridLayout.transform);
            tile.gameObject.SetActive(true);
            tile.TileType = textureData.GetTileType(i);
            tile.OnClick += (tileView) => tileView.TileType = selectedTileType;
            tiles.Add(tile);
        }
    }


    private void Create() => createTileTextureScreen.Show((_) => UpdateTilesDropdown());
}
