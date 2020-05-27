using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Domination.Ui
{
    public class EmptySlotUi : MonoBehaviour
    {
        [SerializeField] private Button button = default;


        public void Init(UnityAction clickAction, bool isInteractable)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(clickAction);
            button.interactable = isInteractable;
        }
    }
}
