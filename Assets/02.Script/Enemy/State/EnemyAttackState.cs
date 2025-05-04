using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName) : base(stateMachine, characterController, enemy, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.Agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_triggerCalled)
        {
            if (_enemy.CanAttack())
                _stateMachine.ChangeState(EEnemyState.Recovery);
            else
                _stateMachine.ChangeState(EEnemyState.Trace);
        }
    }

   
}
