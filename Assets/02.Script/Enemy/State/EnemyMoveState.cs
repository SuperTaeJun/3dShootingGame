using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyMoveState : EnemyState
{
    private float _patrolRadius = 5f;
    private float _waitTime = 2f;

    private Vector3 Destination;
    private bool isMoving = false;

    public EnemyMoveState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.Agent.isStopped = false;
        _enemy.Agent.speed = _enemy.Data.MoveSpeed;

        PickNewDestination();
        _enemy.Agent.SetDestination(Destination);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(_enemy.CanDetect())
        {
            _stateMachine.ChangeState(EEnemyState.Recovery);
            return;
        }


        _enemy.transform.rotation = _enemy.ForwardTarget(_enemy.Agent.steeringTarget);

        if (_enemy.Agent.remainingDistance <= _enemy.Agent.stoppingDistance + 0.05f)
        {
            _stateMachine.ChangeState(EEnemyState.Idle);
        }


        //if (_enemy.GetDistanceToPlayer() < _enemy.Data.DetectRange)
        //{
        //    _stateMachine.ChangeState(EEnemyState.Trace);
        //}

        //if (isMoving)
        //{
        //    MoveToTarget();
        //}
        //else
        //{
        //    if (_stateTimer <= 0)
        //    {
        //        PickNewDestination();
        //        _stateTimer = _waitTime;
        //    }
        //}
    }
    void PickNewDestination()
    {
        Vector2 randomOffset = Random.insideUnitCircle * _patrolRadius;
        Destination = _enemy.StartPos + new Vector3(randomOffset.x, 0, randomOffset.y);
        isMoving = true;
    }

}
