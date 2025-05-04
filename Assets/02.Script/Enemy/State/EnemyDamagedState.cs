using UnityEngine;

public class EnemyDamagedState : EnemyState
{
    public EnemyDamagedState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _characterController.Move(Vector3.zero);
        _stateTimer = _enemy.Data.SturnTime;

        _enemy.Agent.isStopped = true;
        _enemy.Agent.ResetPath();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_stateTimer <= 0)
        {
            _stateMachine.ChangeState(EEnemyState.Trace);
        }

        if (_enemy.CurrentHealth <= 0)
        {
            _stateMachine.ChangeState(EEnemyState.Dead);
        }
    }
}
