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

        _stateTimer = _enemy.SturnTime;
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
            _stateMachine.ChangeState(_enemy.TraceState);
        }

        if(_enemy.Health <=0)
        {
            _stateMachine.ChangeState(_enemy.DeadState);
        }
    }
}
