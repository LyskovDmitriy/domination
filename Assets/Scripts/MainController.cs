using Domination.Data;
using Domination.Ui;
using Domination.EventsSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Domination
{
    public class MainController : MonoBehaviour
    {
        private const string GameMapSceneName = "GameMap";
        private const string BattlefieldSceneName = "Battlefield";

        private GameMap gameMap;
        private EventsAggregator aggregator;


        private void Awake()
        {
            aggregator = new EventsAggregator();
            aggregator.Subscribe(typeof(PlayerAttackSettlment), OnPlayerAttackedSettlment);
            UiController.Init(aggregator);

            StartCoroutine(LoadGameMap(null));
        }

        private IEnumerator LoadGameMap(LevelData data)
        {
            yield return SceneManager.LoadSceneAsync(GameMapSceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameMapSceneName));
            gameMap = FindObjectOfType<GameMap>();
            gameMap.Init(data, aggregator);
        }

        private void OnPlayerAttackedSettlment(IMessage message)
        {
            PlayerAttackSettlment attackMessage = (PlayerAttackSettlment)message;
            StartCoroutine(LoadBattleScene());
        }

        private IEnumerator LoadBattleScene()
        {
            var data = gameMap.GetData();
            gameMap = null;

            yield return SceneManager.UnloadSceneAsync(GameMapSceneName);
            yield return SceneManager.LoadSceneAsync(BattlefieldSceneName, LoadSceneMode.Additive);

            yield return SceneManager.UnloadSceneAsync(BattlefieldSceneName);
            yield return LoadGameMap(data);
            //Set camera position to attacked settlment position
        }
    }
}
