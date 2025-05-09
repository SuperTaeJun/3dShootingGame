using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveState : EnemyState
{
    private Vector3 Destination;

    public EnemyMoveState(EnemyStateMachine stateMachine , Enemy enemy, string animBoolName) : base(stateMachine, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.Agent.isStopped = false;
        _enemy.Agent.speed = _enemy.Data.MoveSpeed;

        //PickNewDestination();
        //_enemy.Agent.SetDestination(Destination);
        MoveToBase();
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

        if (_enemy.Agent.isActiveAndEnabled && _enemy.Agent.isOnNavMesh)
        {
            if (_enemy.Agent.remainingDistance <= _enemy.Agent.stoppingDistance + 0.05f)
            {
                _stateMachine.ChangeState(EEnemyState.Idle);
            }
        }
    }
    private void MoveToBase()
    {
        if (_enemy.BaseTarget != null)
        {
            _enemy.Agent.SetDestination(_enemy.BaseTarget.position);
        }
        else
        {
            Debug.LogWarning("BaseTarget is not assigned on Enemy.");
        }
    }

    void PickNewDestination()
    {
        float maxDistance = 80f; // 원하는 이동 거리 범위
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
            randomDirection.y = 0; // 수평 방향만

            Vector3 randomPoint = _enemy.transform.position + randomDirection;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                Destination = hit.position;
                return;
            }
        }

        Destination = _enemy.transform.position; // fallback
    }

}
