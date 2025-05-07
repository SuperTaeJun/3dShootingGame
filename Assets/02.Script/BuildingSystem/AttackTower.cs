using System.Collections.Generic;
using UnityEngine;

public class AttackTower : BuildableObject
{
    [Header("Turret Settings")]
    public float AttackRange = 20f;
    public float AttackCooldown = 1f;
    public int AttackDamage = 10;

    [Header("Projectile Settings")]
    public GameObject BulletPrefab;     
    public Transform FirePoint;         

    public LayerMask EnemyLayerMask;

    private float _attackTimer = 0f;

    void Update()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0f) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, AttackRange, EnemyLayerMask, QueryTriggerInteraction.Collide);
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

        if (closest != null)
        {
            Shoot(closest);
            _attackTimer = AttackCooldown;
        }
    }

    private void Shoot(Enemy target)
    {

        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        GameObject bulletObj = Instantiate(BulletPrefab, FirePoint.position, Quaternion.LookRotation(dir));
        var bullet = bulletObj.GetComponent<Bullet>();
        bullet.Initialize(dir, gameObject);


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
