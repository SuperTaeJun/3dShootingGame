using System.Collections.Generic;
using UnityEngine;

public class SingleShotTower : BaseAttackTower
{
    public GameObject BulletPrefab;
    public Transform FirePoint;

    protected override void Shoot(Enemy target)
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        //GameObject bulletObj = Instantiate(BulletPrefab, FirePoint.position, Quaternion.LookRotation(dir));
        GameObject bulletObj = ObjectPool.Instance.GetObject(BulletPrefab, FirePoint.position, Quaternion.LookRotation(dir));
        bulletObj.GetComponent<Bullet>().Initialize(dir, Damage, gameObject);
    }
}
