using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyShield : MonoBehaviour, IDamageable
{
    private Enemy _enemy;

    float _health;
    FractureExplosion explosion;
    private List<GameObject> fragments = new List<GameObject>();
    private float destroyTime = 5f;
    private bool isDestroyed = false;
    private void Awake()
    {
        _health = 100f;
        explosion = GetComponent<FractureExplosion>();
        _enemy = GetComponentInParent<Enemy>();
    }
    private void Start()
    {
        _enemy.Animator.SetFloat("Shield", 1);
    }
    public void TakeDamage(Damage damage)
    {
        _health -= damage.Value;
        if (_health <= 0 && !isDestroyed)
        {
            _enemy.Animator.SetFloat("Shield", 0);

            fragments = explosion.Explode();
            isDestroyed = true;

            foreach (var frag in fragments)
            {
                frag.transform.SetParent(null);
            }
            Invoke(nameof(DisableFragments), destroyTime);
            gameObject.SetActive(false);
            //StartCoroutine(DestroyAll(destroyTime));
        }
    }
    private void DisableFragments()
    {
        foreach (var frag in fragments)
            frag.SetActive(false);
    }
}
