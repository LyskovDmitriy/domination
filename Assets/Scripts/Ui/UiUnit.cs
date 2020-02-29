using System;
using UnityEngine;

public class UiUnit<HideResult> : MonoBehaviour
{
    private Action<HideResult> onHidden;


    public virtual void Show(Action<HideResult> onHidden = null)
    {
        this.onHidden = onHidden;

        gameObject.SetActive(true);
    }


    public virtual void Hide(HideResult result)
    {
        gameObject.SetActive(false);

        onHidden?.Invoke(result);
    }
}
