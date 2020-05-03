using System.Collections.Generic;
using UnityEngine;
using Domination.Ui;


public class Game : MonoBehaviour
{
    [SerializeField] private Level level = default;
    [SerializeField] private Selector selector = default;


    private void Awake()
    {
        selector.OnTileSelected += OnTileSelected;
        selector.OnTileDeselected += OnTileDeselected;

        level.Create();

        LevelUi.Prefab.Instance.Show(level);
    }


    private void OnTileSelected(Tile selectedTile)
    {
        if (selectedTile.Settlment == null)
        {
            SettlmentViewScreen.Prefab.Instance.Hide(null);
        }
        else
        {
            SettlmentViewScreen.Prefab.Instance.Show(selectedTile.Settlment);
        }
    }


    private void OnTileDeselected()
    {
        SettlmentViewScreen.Prefab.Instance.Hide(null);
    }
}
