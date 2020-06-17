using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Utils.Ui;


namespace Generator.Ui
{
    public class TileTextureScreen : UiUnit
    {
        [SerializeField] private TileView tileViewPrefab = default;
        [SerializeField] private GridLayoutGroup gridLayout = default;
        [SerializeField] private TMP_Dropdown tilesDropdown = default;
        [SerializeField] private CreateTileTextureScreen createTileTextureScreen = default;

        [SerializeField] private Button closeButton = default;
        [SerializeField] private Button saveButton = default;
        [SerializeField] private Button deleteButton = default;
        [SerializeField] private Button createButton = default;

        [SerializeField] private Button tileVariationButton = default;
        [SerializeField] private RectTransform tileVariationsRoot = default;

        private List<TileView> tiles = new List<TileView>();
        private TileTextureData textureData;

        private TileType selectedTileType;

        private GraphicRaycaster raycaster;
        private Action onHidden;


        private void Awake()
        {
            closeButton.onClick.AddListener(Hide);
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
                Button tileVariation = Instantiate(tileVariationButton, tileVariationsRoot);
                tileVariation.gameObject.SetActive(true);
                tileVariation.image.color = TilesContainer.GetTileColor((TileType)type);
                tileVariation.onClick.AddListener(() => selectedTileType = (TileType)type);
            }

            raycaster = GetComponentInParent<GraphicRaycaster>();
        }


        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                var eventData = new PointerEventData(EventSystem.current);
                eventData.position = Input.mousePosition;
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                raycaster.Raycast(eventData, raycastResults);

                foreach (var raycastResult in raycastResults)
                {
                    TileView tileView = raycastResult.gameObject.GetComponent<TileView>();

                    if (tileView != null)
                    {
                        tileView.TileType = selectedTileType;
                    }
                }
            }
        }


        public void Show(Action onHidden)
        {
            base.Show();

            selectedTileType = TileType.None;
            this.onHidden = onHidden;
        }


        public override void Hide()
        {
            base.Hide();

            onHidden?.Invoke();
            onHidden = null;
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
                tiles.Add(tile);
            }
        }


        private void Create() => createTileTextureScreen.Show(UpdateTilesDropdown);
    }
}
