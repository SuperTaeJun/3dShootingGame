using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private Vector3 AttackDir;
    private float AttackMoveSpeed;

    private const float MaxAttackDistance = 60f;
    public EnemyAttackState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName) : base(stateMachine, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        int randomIndex = Random.Range(0, 3);
        _enemy.Animator.SetFloat("AttackIndex", randomIndex);

        AttackMoveSpeed = _enemy.Data.MoveSpeed;

        _enemy.Agent.isStopped = true;
        _enemy.Agent.velocity = Vector3.zero;

        AttackDir = _enemy.transform.position + (_enemy.transform.forward * MaxAttackDistance);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        if (_enemy.ManualRotationActive())
        {
            _enemy.transform.rotation = _enemy.ForwardTarget(_enemy.Player.transform.position);
        }

        if (_enemy.ManualMovementActive())
        {
            _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, AttackDir, AttackMoveSpeed * Time.deltaTime);
        }

        if (_triggerCalled)
        {
            if (_enemy.CanAttack())
                _stateMachine.ChangeState(EEnemyState.Recovery);
            else
                _stateMachine.ChangeState(EEnemyState.Trace);
        }
    }


}
