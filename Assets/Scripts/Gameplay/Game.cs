using UnityEngine;
using Domination.Ui;
using Domination.EventsSystem;
using Utils.Ui;


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
        }


        private void Start()
        {
            level.Create();

            EventsAggregator.TriggerEvent(new ShowUiMessage(ScreenType.LevelUi));
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
                EventsAggregator.TriggerEvent(new ShowUiMessage(ScreenType.SettlmentViewScreen, (screen) =>
                {
                    ((SettlmentViewScreen)screen).Show(connectedSettlment, level);
                }));

                isSettlmentViewShown = true;
            }
        }


        private void OnTileDeselected() => HideSettlmentViewScreen();


        private void HideSettlmentViewScreen()
        {
            if (isSettlmentViewShown)
            {
                EventsAggregator.TriggerEvent(new HideUiMessage(ScreenType.SettlmentViewScreen));
                isSettlmentViewShown = false;
            }
        }
    }
}
