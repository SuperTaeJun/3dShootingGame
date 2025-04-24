using UnityEngine;

public class Bomb : ProjectileBase
{
    //마우스의 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶음,
    // 1. 슈류탄 오브젝트 만들기
    // 2. 오른쪽 버튼 입력 받기
    // 3. 발사 위치에 수류탄 생성하기
    // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기



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
        Debug.DrawLine(center, center + Vector3.up * 0.1f, Color.red, 1.0f); // 중심점 표시
        DebugExtension.DrawSphere(center, Color.red, radius, 10); // ※ 아래에 유틸 제공


        Collider[] hitEnemies = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Enemy"));
        foreach (var enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<Enemy>().TakeDamage(new Damage(10, gameObject, 40f, transform.forward));
        }

        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
