using UnityEngine;

public class Bomb : ProjectileBase
{
    protected override void Awake()
    {
        base.Awake();
    }
    public override void Initialize(Vector3 launchDirection, float force, GameObject owner = null)
    {
        base.Initialize(launchDirection, force, owner);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);

        float radius = 3f;
        Vector3 center = transform.position;
        LayerMask enemyMask = LayerMask.GetMask("Enemy");

        GameObject vfx = GameObject.Instantiate(ExplotionVfxPrefab);
        vfx.transform.position = center;


        //시각화
        Debug.DrawLine(center, center + Vector3.up * 0.1f, Color.red, 1.0f);
        DebugExtension.DrawSphere(center, Color.red, radius, 10);


        Collider[] hitEnemies = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Enemy"));
        foreach (var enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<Enemy>().TakeDamage(new Damage(10, gameObject, 40f, transform.forward));
        }

        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
