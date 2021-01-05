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


        private void Awake()
        {
            EventsAggregator.Subscribe(typeof(PlayerAttackSettlment), OnPlayerAttackedSettlment);
        }

        private IEnumerator Start()
        {
            var operation = SceneManager.LoadSceneAsync(GameMapSceneName, LoadSceneMode.Additive);
            yield return new WaitUntil(() => operation.isDone);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameMapSceneName));
            var gameMap = FindObjectOfType<GameMap>();
            gameMap.Init();
        }

        private void OnDestroy()
        {
            EventsAggregator.Unsubscribe(typeof(PlayerAttackSettlment), OnPlayerAttackedSettlment);
        }

        private void OnPlayerAttackedSettlment(IMessage message)
        {
            PlayerAttackSettlment attackMessage = (PlayerAttackSettlment)message;
            SceneManager.UnloadSceneAsync(GameMapSceneName);
            SceneManager.LoadSceneAsync(BattlefieldSceneName);
        }
    }
}
