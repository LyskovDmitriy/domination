using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Domination.Ui
{
    public class EmptySlotUi : MonoBehaviour
    {
        [SerializeField] private Button button = default;


        public void Init(UnityAction clickAction)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(clickAction);
        }
    }
}
