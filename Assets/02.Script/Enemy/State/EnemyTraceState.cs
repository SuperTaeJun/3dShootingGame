using UnityEngine;

public class EnemyTraceState : EnemyState
{
    private float LastTimeDestination;
    public EnemyTraceState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.Agent.isStopped = false;
        _enemy.Agent.speed = _enemy.Data.TraceSpeed;
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
        if(!_enemy.CanDetect())
        {
            _enemy.Agent.isStopped = true;
            _enemy.Agent.velocity = Vector3.zero;
            _stateMachine.ChangeState(EEnemyState.Idle);
        }
        if (_enemy.CanAttack())
        {
            _stateMachine.ChangeState(EEnemyState.Attack);
        }

        ////공격범위에 들어오면 어택
        //if (_enemy.GetDistanceToPlayer() <= _enemy.Data.AttackRange)
        //{
        //    Debug.Log("공격");
        //    _stateMachine.ChangeState(EEnemyState.Attack);
        //}
        ////플레이어와 멀어지면 리턴
        //if (_enemy.GetDistanceToPlayer() >= _enemy.Data.ReturnRange)
        //{
        //    _stateMachine.ChangeState(EEnemyState.Return);
        //}

        //_enemy.Agent.SetDestination(_enemy.Player.transform.position);
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
