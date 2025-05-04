using UnityEngine;

public class EnemyRecoveryState : EnemyState
{
    public EnemyRecoveryState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.Agent.isStopped = true;
        _enemy.Agent.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        _enemy.transform.rotation = _enemy.ForwardTarget(_enemy.Player.transform.position);

        if (_triggerCalled)
        {
            _stateMachine.ChangeState(EEnemyState.Trace);
        }

    }
}
