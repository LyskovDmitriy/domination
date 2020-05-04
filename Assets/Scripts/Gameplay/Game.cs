using System.Collections.Generic;
using UnityEngine;
using Domination.Ui;


public class Game : MonoBehaviour
{
    [SerializeField] private Level level = default;
    [SerializeField] private Selector selector = default;

    private bool isSettlmentViewShown;


    private void Awake()
    {
        selector.OnTileSelected += OnTileSelected;
        selector.OnTileDeselected += OnTileDeselected;

        level.Create();

        LevelUi.Prefab.Instance.Show(level);
    }


    private void OnTileSelected(Tile selectedTile)
    {
        Settlment connectedSettlment = selectedTile.Settlment;

        if (connectedSettlment == null)
        {
            HideSettlmentViewScreen();
        }
        else if ((connectedSettlment != null) && level.Player.HasSettlment(selectedTile.Settlment.Id))
        {
            SettlmentViewScreen.Prefab.Instance.Show(connectedSettlment);
            isSettlmentViewShown = true;
        }
    }


    private void OnTileDeselected() => HideSettlmentViewScreen();


    private void HideSettlmentViewScreen()
    {
        if (isSettlmentViewShown)
        {
            SettlmentViewScreen.Prefab.Instance.Hide(null);
            isSettlmentViewShown = false;
        }
    }
}
