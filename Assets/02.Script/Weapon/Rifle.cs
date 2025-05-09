using UnityEngine;
using System.Collections;

public class Rifle : WeaponBase
{
    [Header("Fire Settings")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _bombPrefab;


    [Header("Bomb Settings")]
    [SerializeField] private float _baseThrowPower = 5f;
    [SerializeField] private float _maxThrowPower = 30f;
    [SerializeField] private float _bombChargeRate = 10f;
    [SerializeField] private GameObject ChargingParticle;

    [Header("Scatter Settings")]
    [SerializeField] private float _scatterRadius = 2f;

    private float _distanceToSphere;

    private float _cooldownTimer = 0f;

    protected override void Start()
    {
        base.Start();
        _distanceToSphere = _data.AttackRange;
    }
    public override void Attack()
    {
        HandleBulletInput();
    }

    private void HandleBulletInput()
    {
        _cooldownTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && _cooldownTimer <= 0f && _player.CurrentBulletNum > 0)
        {

            FireOnce();
            _cooldownTimer = _data.FireRate;
        }
    }
    private void FireOnce()
    {
        OnTriggerFireStart.Invoke();

        UseCameraShake();
        _player.UseBullet();

        Vector3 dir = GetFireDirection();
        //GameObject bullet = Instantiate(
        //    _bulletPrefab,
        //    _attackPos.position,
        //    Quaternion.LookRotation(dir)
        //);
        GameObject bullet = ObjectPool.Instance.GetObject(_bulletPrefab, _attackPos.position, Quaternion.LookRotation(dir));
        bullet.GetComponent<Bullet>().Initialize(dir, Data.Damage,_player.gameObject);
    }

    // 지금 카메라 모드에따라서 다른 방식으로 방향을구해줌 + 반동도 추가됨
    private Vector3 GetFireDirection()
    {
        if (_currentCameraType == ECameraType.QuarterView)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
            {
                Vector3 targetPos = hit.point;
                targetPos.y = _attackPos.position.y; // 총구 높이와 일치

                Vector3 dir = (targetPos - _attackPos.position).normalized;
                return dir;
            }
            return _attackPos.forward; // fallback
        }
        else
        {
            TraceUnderCrosshair(out _traceHitResult, _data.AttackRange);
            return CalculateScatterDirection(_attackPos.position);
        }
    }
    //반동 함수
    private Vector3 CalculateScatterDirection(Vector3 startPos)
    {
        //정규화
        Vector3 forward = (_traceHitResult.point - startPos).normalized;

        Vector3 sphereCenter = startPos + forward * _distanceToSphere;
        Vector3 randomOffset = Random.insideUnitSphere * _scatterRadius;
        Vector3 finalPos = sphereCenter + randomOffset;
        return (finalPos - startPos).normalized;
    }

}
