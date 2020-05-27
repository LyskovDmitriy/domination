using UnityEngine;
using Domination.Ui;


namespace Domination
{
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

            LevelUi.Prefab.Instance.Show();
        }


        private void OnTileSelected(Tile selectedTile)
        {
            Settlment connectedSettlment = selectedTile.Settlment;

            if ((connectedSettlment == null) || selectedTile.IsInFog)
            {
                HideSettlmentViewScreen();
            }
            else if (connectedSettlment != null)
            {
                SettlmentViewScreen.Prefab.Instance.Show(connectedSettlment, level.Player);
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
}
