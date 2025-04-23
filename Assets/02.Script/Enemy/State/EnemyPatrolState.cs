using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyPatrolState : EnemyState
{
    private float _patrolRadius = 5f;
    private float _waitTime = 2f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    public EnemyPatrolState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _waitTime;
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
            _stateMachine.ChangeState(EEnemyState.Trace);
        }

        if (isMoving)
        {
            MoveToTarget();
        }
        else
        {
            if (_stateTimer <= 0)
            {
                PickNewDestination();
                _stateTimer = _waitTime;
            }
        }
    }
    void PickNewDestination()
    {
        Vector2 randomOffset = Random.insideUnitCircle * _patrolRadius;
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
