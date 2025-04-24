using System.Collections;
using UnityEngine;

public class Drum : MonoBehaviour, IDamageable
{
    [Header("Setting")]
    [SerializeField]private float _health = 100;
    [SerializeField] private float _power = 30;
    private Rigidbody _rigidbody;
    private float destroyTime = 5f;

    private bool isDestroyed = false;
    [Header("Prefabs")]
    [SerializeField] private GameObject ExplosionPrefab;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private IEnumerator DestoryRoutin(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);

    }

    public void TakeDamage(Damage damage)
    {
        _health -= damage.Value;
        if (_health <= 0 && !isDestroyed)
        {
            Debug.Log("파괴");

            isDestroyed = true;

            float radius = 6f;
            Vector3 center = transform.position;
            LayerMask enemyMask = LayerMask.GetMask("Enemy");


            GameObject vfx = GameObject.Instantiate(ExplosionPrefab);
            vfx.transform.position = center;


            //시각화
            Debug.DrawLine(center, center + Vector3.up * 0.1f, Color.red, 1.0f); // 중심점 표시
            DebugExtension.DrawSphere(center, Color.red, radius, 10); // ※ 아래에 유틸 제공


            Collider[] hitObjects = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Enemy", "Player", "Prop"));
            foreach (var obj in hitObjects)
            {
                obj.gameObject.GetComponent<IDamageable>().TakeDamage(new Damage(100, gameObject, 40f, transform.forward));

                Drum drum = obj.gameObject.GetComponent<Drum>();
                if(drum)
                {
                    drum.RandAddForce();
                }
            }

            RandAddForce();
        }
    }

    private void RandAddForce()
    {
        // 랜덤 방향 + 위쪽 힘 섞기
        Vector3 randomDir = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 1f), // 위쪽으로 좀 더
            Random.Range(-1f, 1f)
        ).normalized;

        _rigidbody.AddForce(randomDir * _power, ForceMode.Impulse);
        StartCoroutine(DestoryRoutin(destroyTime));
    }
}
