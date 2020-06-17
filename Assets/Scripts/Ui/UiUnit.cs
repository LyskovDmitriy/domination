using UnityEngine;
using System;


namespace Utils.Ui
{
    public class UiUnit : MonoBehaviour
    {
        public Action<UiUnit> OnHidden;

        public bool IsShown { get; private set; }


        public virtual void Show()
        {
            IsShown = true;
            gameObject.SetActive(true);
        }


        public virtual void Hide()
        {
            gameObject.SetActive(false);
            IsShown = false;
            OnHidden?.Invoke(this);
        }
    }
}
