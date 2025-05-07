using System.Collections.Generic;
using UnityEngine;

public class AttackTower : BuildableObject
{
    [Header("Turret Settings")]
    public float attackRange = 20f;
    public float attackCooldown = 1f;
    public int attackDamage = 10;

    [Header("Projectile Settings")]
    public GameObject bulletPrefab;     
    public Transform firePoint;         

    public LayerMask enemyLayerMask;

    private float attackTimer = 0f;

    void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer > 0f) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayerMask, QueryTriggerInteraction.Collide);
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
            attackTimer = attackCooldown;
        }
    }

    private void Shoot(Enemy target)
    {

        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
        var bullet = bulletObj.GetComponent<Bullet>();
        bullet.Initialize(dir, gameObject);


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
