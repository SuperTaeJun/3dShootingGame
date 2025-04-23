using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) < _enemy.DetectRange)
        {
            _stateMachine.ChangeState(_enemy.TraceState);
        }

    }
}
