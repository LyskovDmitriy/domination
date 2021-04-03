using Domination.Battle.View;
using Domination.LevelLogic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Domination
{
    public class BattleFieldTestSetup : MonoBehaviour
    {
        [SerializeField] private UnitsGroup[] attackingUnits = default;
        [SerializeField] private UnitsGroup[] defendingUnits = default;
        [SerializeField] private int wallLevel = default;
        [SerializeField] private TileType settlmentTileType = default;


        private IEnumerator Start()
        {
            yield return SceneManager.LoadSceneAsync(GameConstants.BATTLEFIELD_SCENE_NAME, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameConstants.BATTLEFIELD_SCENE_NAME));

            var settlment = new Village(Vector2Int.zero);
            settlment.Build(BuildingType.Wall, wallLevel);

            var battlefiled = FindObjectOfType<BattlefieldView>();
            battlefiled.Init(RecruitmentUtils.CreateArmy(attackingUnits), RecruitmentUtils.CreateArmy(defendingUnits), settlment, new Tile(settlmentTileType));
        }
    }
}
