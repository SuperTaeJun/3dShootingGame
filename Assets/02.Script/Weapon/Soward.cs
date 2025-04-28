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

        if (Input.GetMouseButtonDown(0)) TryAttack();
    }
    void TryAttack()
    {


        LayerMask targetLayer = LayerMask.GetMask("Enemy");

        var hitTargets = GetTargetsInSector(_attackRange, angle,targetLayer);

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
    void OnDrawGizmosSelected()
    {
        Vector3 origin = _player.transform.position;
        Vector3 forward = _player.transform.forward;

        // 반경 시각화 (노란색)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, _attackRange);

        // 부채꼴 경계선 계산
        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * forward;
        Vector3 leftDir = Quaternion.Euler(0, -angle * 0.5f, 0) * forward;

        // 경계선 시각화 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + rightDir * _attackRange);
        Gizmos.DrawLine(origin, origin + leftDir * _attackRange);

        // 가운데 방향 시각화 (파란색)
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, origin + forward.normalized * _attackRange);
    }
}
