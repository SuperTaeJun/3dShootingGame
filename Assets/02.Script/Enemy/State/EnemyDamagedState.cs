using UnityEngine;

public class EnemyDamagedState : EnemyState
{

    public EnemyDamagedState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName) : base(stateMachine, enemy, animBoolName)
    {
    }

    public override void Enter()
    {


        base.Enter();

        _enemy.Agent.isStopped = true;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_enemy.CurrentHealth <= 0)
        {
            _stateMachine.ChangeState(EEnemyState.Dead);
        }
        if (_triggerCalled)
        {
            _stateMachine.ChangeState(EEnemyState.Trace);
        }

    }
}
