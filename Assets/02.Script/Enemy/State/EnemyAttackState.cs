using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.Data.AttackRange;
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
            FireRay();
            Debug.Log("공격");
            _stateTimer = _enemy.Data.AttackRange;
        }


        //공격범위보다 멀어지면 추격
        if (_enemy.GetDistanceToPlayer() > _enemy.Data.AttackRange)
        {
            _stateMachine.ChangeState(EEnemyState.Trace);
        }
    }

    private void FireRay()
    {
        Vector3 direction = (_enemy.Player.transform.position - _enemy.transform.position).normalized;

        if (Physics.Raycast(_enemy.transform.position, direction, out RaycastHit hit, _enemy.Data.AttackRange, LayerMask.GetMask("Player")))
        {
            Debug.DrawLine(_enemy.transform.position, hit.point, Color.red, 1f);

            if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                Debug.Log("attack good");
                damageable.TakeDamage(new Damage(_enemy.Data.Damage, _enemy.gameObject, 20f, direction));
            }
        }
        else
        {
            Vector3 endPoint = _enemy.transform.position + direction * _enemy.Data.AttackRange;
            Debug.DrawLine(_enemy.transform.position, endPoint, Color.red, 1f);
        }
    }
}
