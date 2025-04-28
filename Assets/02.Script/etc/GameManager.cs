using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements.Experimental;

public enum EGameState
{
    Ready,
    Run,
    Over
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState _currentState;
    public EGameState CurrentState => _currentState;
    public Action OnChangeGameToReady;
    public Action OnChangeGameToRun;
    public Action OnChangeGameToOver;
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
        ChanageState(EGameState.Ready);

    }

    public void ChanageState(EGameState newState)
    {
        _currentState = newState;

        switch(newState)
        {
            case EGameState.Ready:
                OnChangeGameToReady.Invoke();
                break;
            case EGameState.Run:
                OnChangeGameToRun.Invoke();
                break;
            case EGameState.Over:
                OnChangeGameToOver.Invoke();
                break;
        }
    }
}
