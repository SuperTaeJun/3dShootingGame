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
        _enemy.UiController.SetActiveHealthBar(false);
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
            DropGold();
            ragdolllController.DisableRagdoll();
            ObjectPool.Instance.ReturnToPool(_enemy.gameObject);
        }

    }
    private void DropGold()
    {
        var dropPrefab = _enemy.CurrencyDropPrefab; // 드랍용 골드 프리팹 연결 필요
        if (dropPrefab == null)
        {
            Debug.LogWarning("CurrencyDropPrefab이 세팅 안되어있어요!");
            return;
        }

        GoldItem drop = GameObject.Instantiate(dropPrefab, _enemy.RagdollCenterBone.position, Quaternion.identity).GetComponent<GoldItem>();
        drop.Initialize(ECurrencyType.Gold, Random.Range(5, 11)); // 5~10 골드 랜덤 드랍
    }

}
