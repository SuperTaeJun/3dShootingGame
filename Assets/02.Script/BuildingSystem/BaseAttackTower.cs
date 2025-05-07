using UnityEngine;

public abstract class BaseAttackTower : BuildableObject
{
    public int Damage = 1;
    public float AttackRange = 20f;
    public float AttackCooldown = 1f;
    public int AttackDamage = 10;
    public LayerMask EnemyLayerMask;

    protected float _attackTimer = 0f;

    protected virtual void Update()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0f) return;

        Enemy target = FindClosestEnemy();
        if (target != null && target.CurrentHealth>0)
        {
            Shoot(target);
            _attackTimer = AttackCooldown;
        }
    }

    protected Enemy FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, AttackRange, EnemyLayerMask);
        Enemy closest = null;
        float minDistSqr = float.MaxValue;

        foreach (var hit in hits)
        {
            var e = hit.GetComponentInParent<Enemy>();
            if (e == null) continue;

            float dSqr = (e.transform.position - transform.position).sqrMagnitude;
            if (dSqr < minDistSqr)
            {
                minDistSqr = dSqr;
                closest = e;
            }
        }
        return closest;
    }

    protected abstract void Shoot(Enemy target);
}
