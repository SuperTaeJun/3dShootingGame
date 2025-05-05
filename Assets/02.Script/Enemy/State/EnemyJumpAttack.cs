using UnityEngine;

public class EnemyJumpAttack : EnemyState
{
    private Vector3 _lastPos;
    private float _jumpAttackSpeed;

    private GameObject _landingZoneVfx;
    public EnemyJumpAttack(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName) : base(stateMachine, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _lastPos = _enemy.Player.transform.position;

        _enemy.Agent.isStopped = true;
        _enemy.Agent.velocity = Vector3.zero;

        float distanceToPlayer = Vector3.Distance(_lastPos, _enemy.transform.position);
        _jumpAttackSpeed = distanceToPlayer / 1;

        if (_enemy.LandingZonePrefab != null)
        {
            Vector3 spawnPos = _lastPos;
            spawnPos.y -= 0.5f; 

            _landingZoneVfx = GameObject.Instantiate(_enemy.LandingZonePrefab, spawnPos, Quaternion.identity);
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (_landingZoneVfx != null)
            GameObject.Destroy(_landingZoneVfx); // 혹시 남아있으면 제거
    }

    public override void Update()
    {
        base.Update();
        Vector3 myPos = _enemy.transform.position;
        _enemy.Agent.enabled = !_enemy.ManualMovementActive();

        if(_enemy.ManualMovementActive() )
        {
            _enemy.transform.position = Vector3.MoveTowards(myPos, _lastPos, _jumpAttackSpeed* Time.deltaTime);
        }

        if(_triggerCalled)
            _stateMachine.ChangeState(EEnemyState.Move);
    }
}
