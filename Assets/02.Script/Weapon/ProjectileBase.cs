using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected Rigidbody rb;
    public float damage;
    protected GameObject owner;

    [Header("Prefabs")]
    [SerializeField] protected GameObject ExplotionVfxPrefab;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public virtual void Initialize(Vector3 launchDirection, float force, GameObject owner = null)
    {
        this.owner = owner;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(launchDirection.normalized * force, ForceMode.Impulse);
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
    }

}
