using System;
using Unity.VisualScripting;
using UnityEngine;

public class UiOptionPopup : UiPopup
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
        }
    }

    public override void Open(Action closeCallback = null)
    {
        base.Open(closeCallback);
    }
    public override void Close()
    {
        base.Close();
    }

    public void OnClickContinueButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Close();
        GameManager.Instance.ChanageState(EGameState.Run);

    }
    public void OnClickRetryButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Close();
        GameManager.Instance.ChanageState(EGameState.Ready);
    }

    public void OnClickQuitButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
    public void OnClickCreditButton()
    {
        PopupManager.Instance.Open(EPopupType.UI_CraditPopup.ToString());
    }

}
