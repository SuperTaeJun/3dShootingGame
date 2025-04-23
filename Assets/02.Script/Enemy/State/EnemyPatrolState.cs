using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyPatrolState : EnemyState
{
    public float patrolRadius = 5f;
    public float waitTime = 2f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float waitTimer = 0f;
    public EnemyPatrolState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
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
        if (Vector3.Distance(_enemy.Player.transform.position, _enemy.transform.position) < _enemy.DetectRange)
        {
            _stateMachine.ChangeState(_enemy.TraceState);
        }

        if (isMoving)
        {
            MoveToTarget();
        }
        else
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                PickNewDestination();
                waitTimer = 0f;
            }
        }
    }
    void PickNewDestination()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        targetPosition = _enemy.StartPos + new Vector3(randomOffset.x, 0, randomOffset.y);
        isMoving = true;
    }

    void MoveToTarget()
    {
        Vector3 direction = (targetPosition - _enemy.transform.position).normalized;

        _characterController.Move(direction * _enemy.MoveSpeed * Time.deltaTime);
        if (Vector3.Distance(_enemy.transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }
}
