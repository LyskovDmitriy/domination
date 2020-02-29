using System;
using UnityEngine;
using UnityEngine.UI;


public class TileView : MonoBehaviour
{
    [SerializeField] private Image image = default;

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
}
