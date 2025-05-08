using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System;


public enum EPopupType
{
    UI_OptionPopup,
    UI_CraditPopup,
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    [Header("팝업 ui 참조")]
    public List<UiPopup> Popups;
    private Stack<UiPopup> _openedPopups = new Stack<UiPopup>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_openedPopups.Count > 0)
            {
                while(true)
                {
                    UiPopup popup = _openedPopups.Pop();

                    bool opend = popup.isActiveAndEnabled;
                    popup.Close();
                    //_openedPopups[_openedPopups.Count - 1].Close();
                    //_openedPopups.RemoveAt(_openedPopups.Count - 1);

                    if (opend || _openedPopups.Count ==0)
                    {
                        break;
                    }
                }

            }
            else
            {
                GameManager.Instance.ChanageState(EGameState.Pause);
                Open(EPopupType.UI_OptionPopup.ToString(), closeCallback: GameManager.Instance.Run);
            }
        }

    }
    public void Open(string popupName, Action closeCallback = null)
    {
        foreach (UiPopup popup in Popups)
        {
            if (popup.gameObject.name == popupName)
            {
                popup.Open(closeCallback);
                _openedPopups.Push(popup);
                break;
            }
        }
    }
    public void Close()
    {

    }

}
