using System.Collections.Generic;
using UnityEngine;

public class Soward : WeaponBase
{

    // 부채꼴 각도
    [SerializeField] private float angle = 30;
    public override void Attack()
    {
        HandleSwoardInput();
    }
    private void HandleSwoardInput()
    {

        if (Input.GetMouseButtonDown(0)) OnTriggerFireStart.Invoke();
    }
    public void TryAttack()
    {

        LayerMask targetLayer = LayerMask.GetMask("Enemy");

        var hitTargets = GetTargetsInSector(_data.AttackRange, angle,targetLayer);

        foreach (var target in hitTargets)
        {
            
            if(target.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(new Damage(_data.Damage, _player.gameObject, 20f, _player.transform.forward));
            }
        }
    }

    List<Collider> GetTargetsInSector(float radius, float angle, LayerMask targetLayer)
    {
        Vector3 origin = _player.transform.position;
        Vector3 forward = _player.transform.forward;

        List<Collider> result = new List<Collider>();

        Collider[] colliders = Physics.OverlapSphere(origin, radius, targetLayer);
        DebugExtension.DrawSphere(_player.transform.position, Color.red, radius, 3);
        float cosHalfAngle = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);

        foreach (var col in colliders)
        {
            Vector3 toTarget = col.transform.position - origin;

            Vector3 dirToTarget = toTarget.normalized;
            float dot = Vector3.Dot(forward.normalized, dirToTarget);

            if (dot >= cosHalfAngle)
            {
                result.Add(col);
            }
        }

        return result;
    }

}
