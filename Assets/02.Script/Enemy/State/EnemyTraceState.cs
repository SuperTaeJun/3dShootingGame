using UnityEngine;

public class EnemyTraceState : EnemyState
{
    private float LastTimeDestination;
    public EnemyTraceState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName) : base(stateMachine, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.Agent.isStopped = false;
        _enemy.Agent.speed = _enemy.Data.TraceSpeed;
        _enemy.Agent.destination = _enemy.Player.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        _enemy.transform.rotation = _enemy.ForwardTarget(_enemy.Agent.steeringTarget);

        if (CanUpdateDestination())
        {
            _enemy.Agent.destination = _enemy.Player.transform.position;
        }
        if (!_enemy.CanDetect())
        {
            if (!(_enemy.EnemyType == EEnemyType.Chase))
            {
                _enemy.Agent.isStopped = true;
                _enemy.Agent.velocity = Vector3.zero;
            }
            _stateMachine.ChangeState(EEnemyState.Idle);
        }
        if(_enemy.CanDoJumpAttack() && _enemy.EnemyType == EEnemyType.Jump)
        {
            _stateMachine.ChangeState(EEnemyState.JumpAttack);
        }
        if (_enemy.CanAttack())
        {
            _stateMachine.ChangeState(EEnemyState.Attack);
        }

    }
    private bool CanUpdateDestination()
    {
        if (Time.time > LastTimeDestination + 0.25f)
        {
            LastTimeDestination = Time.time;
            return true;
        }
        return false;
    }
}
