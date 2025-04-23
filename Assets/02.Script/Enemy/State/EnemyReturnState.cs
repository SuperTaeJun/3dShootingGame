using UnityEngine;

public class EnemyReturnState : EnemyState
{
    Transform _startPosition;
    public EnemyReturnState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName,Transform startPosition) : base(stateMachine, characterController, enemy, animBoolName)
    {
        _startPosition = startPosition;
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

        if (Vector3.Distance(_enemy.transform.position, _startPosition.position) <= _characterController.minMoveDistance)
        {
            _enemy.transform.position = _startPosition.position;
            _stateMachine.ChangeState(_enemy.IdleState);
        }

        if(Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) < _enemy.DetectRange)
        {
            _stateMachine.ChangeState(_enemy.TraceState);
        }

        Vector3 dir = _startPosition.position - _enemy.transform.position;
        _characterController.Move(dir * _enemy.MoveSpeed * Time.deltaTime);


    }
}
