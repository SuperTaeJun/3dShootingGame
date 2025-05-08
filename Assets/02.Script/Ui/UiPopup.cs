using System;
using UnityEngine;

public class UiPopup : MonoBehaviour
{
    private Action _closeCallback;
    public virtual void Open(Action closeCallback = null)
    {
        _closeCallback = closeCallback;

        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        _closeCallback?.Invoke();
        gameObject.SetActive(false);
    }


}
