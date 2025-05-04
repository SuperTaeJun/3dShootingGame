using UnityEngine;

public class EnemyAnimEvents : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void StartManualMovement() => _enemy.AcitveManualMovement(true);
    private void StopManualMovement() => _enemy.AcitveManualMovement(false);
    private void OnAnimTrigger() => _enemy.AnimTrigger();
    private void OnTryAttack()
    {
        _enemy.TryAttack();
    }


}
