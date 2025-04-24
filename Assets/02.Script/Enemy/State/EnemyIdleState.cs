using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.EnemyData.PatrolTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_enemy.GetDistanceToPlayer() < _enemy.EnemyData.DetectRange)
        {
            _stateMachine.ChangeState(EEnemyState.Trace);
        }

        if (_stateTimer < 0)
        {
            _stateMachine.ChangeState(EEnemyState.Patrol);
        }
    }
}
