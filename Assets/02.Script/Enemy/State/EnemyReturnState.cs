using UnityEngine;

public class EnemyReturnState : EnemyState
{
    Vector3 _startPosition;
    public EnemyReturnState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName, Vector3 startPosition) : base(stateMachine, characterController, enemy, animBoolName)
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

        if (Vector3.Distance(_enemy.transform.position, _startPosition) <= _characterController.minMoveDistance)
        {
            _enemy.transform.position = _startPosition;
            _stateMachine.ChangeState(EEnemyState.Idle);
        }

        if(Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) < _enemy.EnemyData.DetectRange)
        {
            _stateMachine.ChangeState(EEnemyState.Trace);
        }

        //Vector3 dir = _startPosition - _enemy.transform.position;
        //_characterController.Move(dir * _enemy.EnemyData.MoveSpeed * Time.deltaTime);
        _enemy._agent.SetDestination(_startPosition);

    }
}
