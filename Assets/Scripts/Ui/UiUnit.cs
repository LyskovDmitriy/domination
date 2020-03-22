using System;
using UnityEngine;

public class UiUnit<HideResult> : MonoBehaviour
{
    private Action<HideResult> onHidden;


    public bool IsShown { get; private set; }


    public virtual void Show(Action<HideResult> onHidden = null)
    {
        this.onHidden = onHidden;

        IsShown = true;
        gameObject.SetActive(true);
    }


    public virtual void Hide(HideResult result)
    {
        gameObject.SetActive(false);
        IsShown = false;

        onHidden?.Invoke(result);
    }
}
