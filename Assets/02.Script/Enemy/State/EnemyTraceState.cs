using UnityEngine;

public class EnemyTraceState : EnemyState
{
    public EnemyTraceState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
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

        //공격범위에 들어오면 어택
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) <= _enemy.AttackRange)
        {
            Debug.Log("공격");
            _stateMachine.ChangeState(_enemy.AttackState);
        }
        //플레이어와 멀어지면 리턴
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) >= _enemy.ReturnRange)
        {
            _stateMachine.ChangeState(_enemy.ReturnState);
        }

        Vector3 dir = _enemy.Player.transform.position - _enemy.transform.position;
        _characterController.Move(dir * _enemy.MoveSpeed * Time.deltaTime);

    }
}
