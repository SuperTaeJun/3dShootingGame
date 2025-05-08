using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements.Experimental;

public enum EGameState
{
    Ready,
    Run,
    Pause,
    Over
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState _currentState;
    public EGameState CurrentState => _currentState;
    public Action<EGameState> OnChangeGameStatea;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        ChanageState(EGameState.Run);

    }
    public void ChanageState(EGameState newState)
    {
        _currentState = newState;
        OnChangeGameStatea.Invoke(_currentState);

        switch(_currentState)
        {
            case EGameState.Pause:
                Pause();
                break;
        }

    }

    public void Run()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        ChanageState(EGameState.Run);

    }

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;

    }

    public void Restary()
    {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;

        int curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(curSceneIndex);
        ChanageState(EGameState.Run);
    }
}
