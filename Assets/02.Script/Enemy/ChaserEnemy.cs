using UnityEngine;

public class ChaserEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        // 불필요한 상태 제거
        _statesMap[EEnemyState.Idle] = new EnemyTraceState(_stateMachine, _characterController, this, "Trace");
        _statesMap[EEnemyState.Patrol] = _statesMap[EEnemyState.Trace];
        _statesMap[EEnemyState.Return] = _statesMap[EEnemyState.Trace];
    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.ChangeState(EEnemyState.Trace); // 시작하자마자 추격 시작
    }

    protected override void Update()
    {
        base.Update();
    }
}
