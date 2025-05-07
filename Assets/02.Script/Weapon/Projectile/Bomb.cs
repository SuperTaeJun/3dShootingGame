using UnityEngine;

public class Bomb : ProjectileBase
{
    [Header("Bomb Settings")]
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private LayerMask _damageableLayers;

    protected override void OnProjectileImpact(Collision collision)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius, _damageableLayers);
        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;

            EnemyShield shield = hitCollider.GetComponent<EnemyShield>();
            if (shield)
            {
                shield.TakeDamage(new Damage(_damage, _owner, 10f, directionToTarget));
                continue;
            }

            if (hitCollider.GetComponentInParent<IDamageable>() is IDamageable target)
            {
                target.TakeDamage(new Damage(_damage, _owner, 10f, directionToTarget));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}



