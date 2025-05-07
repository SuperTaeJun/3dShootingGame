using UnityEngine;

public class BombTower : BaseAttackTower
{
    public GameObject BombPrefab;
    public Transform FirePoint;

    protected override void Shoot(Enemy target)
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

        GameObject bombObj = Instantiate(BombPrefab, FirePoint.position, Quaternion.LookRotation(dir));
        var bomb = bombObj.GetComponent<Bomb>();
        bomb.Initialize(dir, Damage, gameObject);
    }
}
