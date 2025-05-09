using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyState
{
    private readonly RagdolllController _ragdollController;

    // Fade 제어용 머티리얼 리스트
    private List<Material> _materials;

    public EnemyDeadState(EnemyStateMachine stateMachine , Enemy enemy, string animBoolName, RagdolllController ragdolllController) : base(stateMachine, enemy, animBoolName)
    {
        this._ragdollController = ragdolllController;
    }

    public override void Enter()
    {
        base.Enter();

        _stateTimer = _enemy.Data.DeadTime;

        _enemy.Agent.isStopped = true;
        _enemy.UiController.SetActiveHealthBar(false);
        _ragdollController.EnableRagdoll();

        // 3) Fade 조절할 머티리얼들 캐싱
        _materials = new List<Material>();
        foreach (var rend in _enemy.GetComponentsInChildren<Renderer>())
        {
            // 인스턴스 복제된 머티리얼만 건드림
            _materials.Add(rend.material);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        float f = Mathf.Clamp01(_stateTimer / _enemy.Data.DeadTime);
        foreach (var mat in _materials)
        {
            if (mat.HasProperty("_Fade"))
                mat.SetFloat("_Fade", f);
        }


        if (_stateTimer<0)
        {
            DropGold();
            _ragdollController.DisableRagdoll();

            foreach (var mat in _materials)
            {
                if (mat.HasProperty("_Fade"))
                    mat.SetFloat("_Fade", 1f);
            }
            ObjectPool.Instance.ReturnToPool(_enemy.gameObject);
        }

    }
    private void DropGold()
    {
        var dropPrefab = _enemy.CurrencyDropPrefab; 
        if (dropPrefab == null)
        {
            Debug.LogWarning("CurrencyDropPrefab이 세팅 안되어있어요!");
            return;
        }

        GoldItem drop = GameObject.Instantiate(dropPrefab, _enemy.RagdollCenterBone.position, Quaternion.identity).GetComponent<GoldItem>();
        drop.Initialize(ECurrencyType.Gold, Random.Range(5, 11)); 
    }

}
