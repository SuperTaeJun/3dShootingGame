using UnityEngine;

public class Bullet : ProjectileBase
{
    [Header("Bullet Settings")]
    [SerializeField] private float _lifeTime = 3f;


    protected override void OnProjectileImpact(Collision collision)
    {
        EnemyShield shield = collision.collider.GetComponent<EnemyShield>();
        if (shield)
        {
            shield.TakeDamage(new Damage(_damage, _owner, 10f, transform.forward));
            return;
        }

        if (collision.collider.GetComponentInParent<IDamageable>() is IDamageable target)
        {
            target.TakeDamage(new Damage(_damage, _owner, 10f, transform.forward));
        }
    }
}
