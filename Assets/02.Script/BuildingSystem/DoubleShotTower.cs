using UnityEngine;

public class DoubleShotTower : BaseAttackTower
{
    public GameObject BulletPrefab;
    public Transform[] FirePoint;

    protected override void Shoot(Enemy target)
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        // 각 총구에서 직진 방향으로 발사
        GameObject bullet1 = Instantiate(BulletPrefab, FirePoint[0].position, Quaternion.LookRotation(dir));
        bullet1.GetComponent<Bullet>().Initialize(dir, Damage, gameObject);

        GameObject bullet2 = Instantiate(BulletPrefab, FirePoint[1].position, Quaternion.LookRotation(dir));
        bullet2.GetComponent<Bullet>().Initialize(dir, Damage, gameObject);
    }
}
