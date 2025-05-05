using UnityEngine;

public class Bomb : ProjectileBase
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float explosionForce = 40f;
    [SerializeField] private int explosionDamage = 10;
    [SerializeField] private LayerMask damageLayer = default;

    protected override void OnProjectileImpact(Collision collision)
    {
        Vector3 center = transform.position;

        // 디버그 시각화
        Debug.DrawLine(center, center + Vector3.up * 0.1f, Color.red, 1.0f);
        DebugExtension.DrawSphere(center, Color.red, explosionRadius, 10);

        // 범위 내 피해 적용
        Collider[] hitTargets = Physics.OverlapSphere(center, explosionRadius, damageLayer);
        foreach (var target in hitTargets)
        {
            if (target.GetComponentInParent<IDamageable>() is IDamageable damageable)
            {
                damageable.TakeDamage(new Damage(explosionDamage, _owner, explosionForce, transform.forward));
            }
        }
    }
}
