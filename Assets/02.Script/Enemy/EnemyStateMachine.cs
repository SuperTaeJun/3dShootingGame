using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public bool IsInited = false;
    private EnemyState _currentState;
    public EnemyState CurrentState => _currentState;
    private Enemy _enemy;
    private Dictionary<EEnemyState, EnemyState> _stateMap;

    public void InitStateMachine(EnemyState currentState, Enemy enemy, Dictionary<EEnemyState, EnemyState> stateMap)
    {
        _currentState = currentState;
        _stateMap = stateMap;
        _enemy = enemy;
        IsInited = true;
    }
    public void ChangeState(EnemyState state)
    {
        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }
    public void ChangeState(EEnemyState stateType)
    {
        if (_stateMap.TryGetValue(stateType, out var nextState))
        {
            ChangeState(nextState);
        }
        else
        {
            Debug.LogWarning($"없는 상태인데용? 이거 {stateType} ");
        }
    }

    public void Update()
    {
        _currentState.Update();
    }
}
