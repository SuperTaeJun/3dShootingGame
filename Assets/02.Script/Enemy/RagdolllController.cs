using UnityEngine;

public class RagdolllController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();

        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        _animator.enabled = false;
        foreach (var rigid in _rigidbodies)
        {
            rigid.isKinematic = false;
        }
    }

    public void DisableRagdoll()
    {
        _animator.enabled = true;
        foreach (var rigid in _rigidbodies)
        {
            rigid.isKinematic = true;
        }
    }
}
