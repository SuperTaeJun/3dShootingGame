using UnityEngine;

public class EnemyDeadState : EnemyState
{
    RagdolllController ragdolllController;
    public EnemyDeadState(EnemyStateMachine stateMachine, CharacterController characterController, Enemy enemy, string animBoolName, RagdolllController ragdolllController) : base(stateMachine, characterController, enemy, animBoolName)
    {
        this.ragdolllController = ragdolllController;
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.Data.DeadTime;
        ragdolllController.EnableRagdoll();
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
            ragdolllController.DisableRagdoll();
            ObjectPool.Instance.ReturnToPool(_enemy.gameObject);
        }

    }

    
}
