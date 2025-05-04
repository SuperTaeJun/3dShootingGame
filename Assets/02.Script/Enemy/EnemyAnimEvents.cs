using UnityEngine;

public class EnemyAnimEvents : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnAnimTrigger() => _enemy.AnimTrigger();
    private void AttackRay()
    {
        _enemy.TryAttack();
    }


}
