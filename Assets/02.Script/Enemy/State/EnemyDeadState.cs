using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.EnemyData.DeadTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(_stateTimer<0)
        {
            _enemy.gameObject.SetActive(false);
        }

    }

    
}
