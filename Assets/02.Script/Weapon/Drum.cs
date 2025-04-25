using System.Collections;
using System.Collections.Generic;
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

    FractureExplosion explosion;
    private List<GameObject> fragments = new List<GameObject>();
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        explosion = GetComponent<FractureExplosion>();
    }

    public void TakeDamage(Damage damage)
    {
        _health -= damage.Value;
        if (_health <= 0 && !isDestroyed)
        {
            fragments = explosion.Explode();
            isDestroyed = true;

            float radius = 6f;
            Vector3 center = transform.position;

            GameObject vfx = GameObject.Instantiate(ExplosionPrefab);
            vfx.transform.position = center;

            //시각화
            Debug.DrawLine(center, center + Vector3.up * 0.1f, Color.red, 1.0f); 
            DebugExtension.DrawSphere(center, Color.red, radius, 10);

            Collider[] hitObjects = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Enemy", "Player", "Prop"));
            foreach (var obj in hitObjects)
            {
                obj.gameObject.GetComponent<IDamageable>().TakeDamage(new Damage(100, gameObject, 40f, transform.forward));
            }
            DestroyAll();
        }
    }

    private void DestroyAll()
    {
        //오브젝트 풀에 넣으셈
        Destroy(gameObject, destroyTime);
        foreach (var frag in fragments)
        {
            Destroy(frag, destroyTime);
        }
        gameObject.SetActive(false);
    }
}
