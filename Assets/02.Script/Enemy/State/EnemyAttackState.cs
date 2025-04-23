using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.AttackRate;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_stateTimer < 0)
        {
            Debug.Log("공격");
            _stateTimer = _enemy.AttackRate;
        }


        //공격범위보다 멀어지면 추격
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) > _enemy.AttackRange)
        {
            _stateMachine.ChangeState(_enemy.TraceState);
        }
    }
}
