using System;
using UnityEngine;
using UnityEngine.UI;


public class TileView : MonoBehaviour
{
    public event Action<TileView> OnClick;

    [SerializeField] private Image image = default;
    [SerializeField] private Button button = default;

    private TileType tileType;


    public TileType TileType
    {
        get => tileType;
        set
        {
            tileType = value;
            image.color = TilesContainer.GetTileColor(TileType);
        }
    }


    private void Awake()
    {
        button.onClick.AddListener(() => OnClick?.Invoke(this));
    }
}
