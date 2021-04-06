using UnityEngine;
using UnityEngine.UI;


namespace Domination.Battle.Test
{
    public class BattleFieldDebuggerUi : MonoBehaviour
    {
        [SerializeField] private Button toggleDebugViewButton = default;
        [Space]
        [SerializeField] private BattleFieldDebugger controller = default;


        private void Awake()
        {
            toggleDebugViewButton.onClick.AddListener(() => controller.SetActive(!controller.IsActive));
        }
    }
}
