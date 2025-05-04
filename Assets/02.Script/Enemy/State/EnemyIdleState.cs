using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyStateMachine stateMachine,  Enemy enemy, string animBoolName) : base(stateMachine, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.Data.PatrolTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_enemy.CanDetect())
        {
            _stateMachine.ChangeState(EEnemyState.Recovery);
            return;
        }

        if (_stateTimer < 0)
        {
            _stateMachine.ChangeState(EEnemyState.Move);
        }
    }
}
