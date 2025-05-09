using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class ProjectileBase : MonoBehaviour
{
    protected Rigidbody _rb;
    protected GameObject _owner;


    [Header("Stats")]
    [SerializeField] protected float _speed = 50f;
    [SerializeField] protected int _damage = 10;
    private float _lifeTime = 4f;
    private float _timer = 0;

    [Header("Effects")]
    [SerializeField] protected GameObject _explosionVfxPrefab;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        _timer = _lifeTime;
    }

    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        _timer -= Time.deltaTime;

        if(_timer <0)
        {
            ObjectPool.Instance.ReturnToPool(gameObject);
        }

    }
    public virtual void Initialize(Vector3 launchDirection, int damage ,GameObject owner = null)
    {
        this._owner = owner;
        _rb.linearVelocity = launchDirection.normalized * _speed;
        _damage = damage;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        OnProjectileImpact(collision);
        PlayImpactEffect();
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
    

    protected virtual void OnProjectileImpact(Collision collision) { }

    protected void PlayImpactEffect()
    {
        if (_explosionVfxPrefab != null)
        {
            Instantiate(_explosionVfxPrefab, transform.position, Quaternion.identity);
        }
    }

}
