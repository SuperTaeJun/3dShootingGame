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

        foreach (var col in _colliders)
        {
            col.enabled = true;
        }
    }

    public void DisableRagdoll()
    {
        foreach (var rigid in _rigidbodies)
        {
            rigid.isKinematic = true;
        }

        foreach (var col in _colliders)
        {
            // 본체 콜라이더 제외하고 끄기
            if (col.gameObject != this.gameObject)
                col.enabled = false;
        }
    }
}
