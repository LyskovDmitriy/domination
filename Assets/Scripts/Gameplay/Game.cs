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

        private ScreenType settlmentScreenType = ScreenType.None;


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
                if (connectedSettlment.Lord == level.Player)
                {
                    settlmentScreenType = ScreenType.PlayerSettlmentViewScreen;
                    EventsAggregator.TriggerEvent(new ShowUiMessage(ScreenType.PlayerSettlmentViewScreen, (screen) =>
                    {
                        ((PlayerSettlmentViewScreen)screen).Show(connectedSettlment, level);
                    }));
                }
                else
                {
                    settlmentScreenType = ScreenType.EnemySettlmentViewScreen;
                    EventsAggregator.TriggerEvent(new ShowUiMessage(ScreenType.EnemySettlmentViewScreen, (screen) =>
                    {
                        ((EnemySettlmentViewScreen)screen).Show(connectedSettlment, level);
                    }));
                }
            }
        }


        private void OnTileDeselected() => HideSettlmentViewScreen();


        private void HideSettlmentViewScreen()
        {
            if (settlmentScreenType != ScreenType.None)
            {
                EventsAggregator.TriggerEvent(new HideUiMessage(settlmentScreenType));
                settlmentScreenType = ScreenType.None;
            }
        }
    }
}
