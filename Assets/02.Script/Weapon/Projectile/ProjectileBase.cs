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

    [Header("Effects")]
    [SerializeField] protected GameObject _explosionVfxPrefab;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public virtual void Initialize(Vector3 launchDirection , GameObject owner = null)
    {
        this._owner = owner;
        _rb.linearVelocity = launchDirection.normalized * _speed;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        OnProjectileImpact(collision);
        PlayImpactEffect();
        Destroy(gameObject);
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
