using Domination.Data;
using Domination.Ui;
using Domination.EventsSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Ui;
using Domination.Battle.View;


namespace Domination
{
    public class MainController : MonoBehaviour
    {
        private const string GameMapSceneName = "GameMap";

        private EventsAggregator aggregator;
        private Level level;


        private void Awake()
        {
            aggregator = new EventsAggregator();
            aggregator.Subscribe(typeof(PlayerAttackSettlment), OnPlayerAttackedSettlment);
            UiController.Init(aggregator);

            level = new Level(aggregator);

            StartCoroutine(LoadGameMap(null));
        }

        private IEnumerator LoadGameMap(LevelData data)
        {
            yield return SceneManager.LoadSceneAsync(GameMapSceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameMapSceneName));
            var gameMap = FindObjectOfType<GameMap>();
            gameMap.Init(level, aggregator);
        }

        private void OnPlayerAttackedSettlment(IMessage message)
        {
            var attackMessage = (PlayerAttackSettlment)message;
            aggregator.TriggerEvent(new HideUiMessage(ScreenType.LevelUi));
            StartCoroutine(LoadBattleScene(attackMessage.SettlmentId));
        }

        private IEnumerator LoadBattleScene(uint attackedSettlmentId)
        {
            yield return SceneManager.UnloadSceneAsync(GameMapSceneName);
            
            yield return SceneManager.LoadSceneAsync(GameConstants.BATTLEFIELD_SCENE_NAME, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameConstants.BATTLEFIELD_SCENE_NAME));

            SetupBattleFieldScene(attackedSettlmentId);

            //yield return SceneManager.UnloadSceneAsync(BattlefieldSceneName);
            //yield return LoadGameMap(data);
            //Set camera position to attacked settlment position
        }

        private void SetupBattleFieldScene(uint attackedSettlmentId)
        {
            var battlefiled = FindObjectOfType<BattlefieldView>();

            var attackedSettlment = level.GetSettlment(attackedSettlmentId);

            var attackingArmy = level.Player.GetSettlmentArmy(attackedSettlment);
            var defendingArmy = attackedSettlment.Lord.GetSettlmentArmy(attackedSettlment);

            var settlmentTile = level.Map[attackedSettlment.Position.x, attackedSettlment.Position.y];

            battlefiled.Init(attackingArmy, defendingArmy, attackedSettlment, settlmentTile);
            battlefiled.StartAttack();
        }
    }
}
