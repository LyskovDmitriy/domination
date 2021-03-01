using UnityEngine;
using Domination.Ui;
using Domination.EventsSystem;
using Utils.Ui;
using Domination.LevelView;


namespace Domination
{
    public class GameMap : MonoBehaviour
    {
        [SerializeField] private Level level = default;
        [SerializeField] private Selector selector = default;

        private ScreenType settlmentScreenType = ScreenType.None;


        private void Awake()
        {
            selector.OnTileSelected += OnTileSelected;
            selector.OnTileDeselected += OnTileDeselected;
        }

        public void Init()
        {
            level.Create();

            EventsAggregator.TriggerEvent(new ShowUiMessage(ScreenType.LevelUi));
        }

        private void OnTileSelected(TileView selectedTile)
        {
            Settlment connectedSettlment = level.FindSettlment(selectedTile.Position)?.Settlment;

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
