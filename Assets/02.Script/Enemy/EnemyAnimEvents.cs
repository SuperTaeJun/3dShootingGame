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
    public void StartManualRotation() => _enemy.AcitveManualRotation(true);
    public void StopManualRotation() => _enemy.AcitveManualRotation(false);
    private void OnAnimTrigger() => _enemy.AnimTrigger();
    private void OnTryAttack() => _enemy.TryAttack();

    private void OnTryJumpAttack() => _enemy.TryJumpAttack();


}
