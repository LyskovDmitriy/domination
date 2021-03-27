using UnityEngine;
using Domination.Ui;
using Domination.EventsSystem;
using Utils.Ui;
using Domination.LevelView;
using Domination.LevelLogic;
using Domination.Data;


namespace Domination
{
    public class GameMap : MonoBehaviour
    {
        [SerializeField] private MapView map = default;
        [SerializeField] private Selector selector = default;

        private ScreenType settlmentScreenType = ScreenType.None;
        private EventsAggregator aggregator;


        private void Awake()
        {
            selector.OnTileSelected += OnTileSelected;
            selector.OnTileDeselected += OnTileDeselected;
        }

        public void Init(Level level, EventsAggregator aggregator)
        {
            this.aggregator = aggregator;
            aggregator.TriggerEvent(new ShowUiMessage(ScreenType.LevelUi));
            map.Create(level);
        }

        public LevelData GetData() => map.GetData();

        private void OnTileSelected(TileView selectedTile)
        {
            Settlment connectedSettlment = map.FindSettlment(selectedTile.Position)?.Settlment;

            if ((connectedSettlment == null) || selectedTile.IsInFog)
            {
                HideSettlmentViewScreen();
            }
            else if (connectedSettlment != null)
            {
                if (connectedSettlment.Lord == map.Player)
                {
                    settlmentScreenType = ScreenType.PlayerSettlmentViewScreen;
                    aggregator.TriggerEvent(new ShowUiMessage(ScreenType.PlayerSettlmentViewScreen, (screen) =>
                    {
                        ((PlayerSettlmentViewScreen)screen).Show(connectedSettlment, map.Player);
                    }));
                }
                else
                {
                    settlmentScreenType = ScreenType.EnemySettlmentViewScreen;
                    aggregator.TriggerEvent(new ShowUiMessage(ScreenType.EnemySettlmentViewScreen, (screen) =>
                    {
                        ((EnemySettlmentViewScreen)screen).Show(connectedSettlment, map.Level);
                    }));
                }
            }
        }

        private void OnTileDeselected() => HideSettlmentViewScreen();

        private void HideSettlmentViewScreen()
        {
            if (settlmentScreenType != ScreenType.None)
            {
                aggregator.TriggerEvent(new HideUiMessage(settlmentScreenType));
                settlmentScreenType = ScreenType.None;
            }
        }
    }
}
