using UnityEngine;
using System.Collections;
public class Shotgun : WeaponBase
{
    [Header("Fire Settings")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _numOfPill;

    [Header("Scatter Settings")]
    [SerializeField] private float _scatterRadius = 2f;
    private float _distanceToSphere;

    private Coroutine _fireCoroutine;

    protected override void Start()
    {
        base.Start();
        _distanceToSphere = _data.AttackRange*2;
    }
    public override void Attack()
    {
        HandleBulletInput();
    }

    private void HandleBulletInput()
    {
        if (Input.GetMouseButtonDown(0)) StartFiring();
        if (Input.GetMouseButtonUp(0)) StopFiring();
    }
    private void StartFiring()
    {
        if (_fireCoroutine == null && _player.CurrentBulletNum > 0)
        {
            _fireCoroutine = StartCoroutine(FireLoop(_data.FireRate));
            _player.IsFiring = true;
        }
    }

    private void StopFiring()
    {
        if (_fireCoroutine != null)
        {
            StopCoroutine(_fireCoroutine);
            _fireCoroutine = null;
            _player.IsFiring = false;
        }
    }
    private IEnumerator FireLoop(float fireRate)
    {
        while (_player.CurrentBulletNum > 0)
        {
            for (int i = 0; i < _numOfPill; ++i)
            {
                OnTriggerFireStart.Invoke();
                _player.UseBullet();

                Vector3 fireDir = GetFireDirection();

                GameObject bulletObj = Instantiate(_bulletPrefab);
                bulletObj.transform.position = _attackPos.position;
                bulletObj.transform.rotation = Quaternion.LookRotation(fireDir);
                Bullet bullet = bulletObj.GetComponent<Bullet>();
                bullet.Initialize(fireDir, _player.gameObject);
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

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

    private IEnumerator SpawnTrail(GameObject trail, Vector3 hitPoint)
    {
        float alpha = 0f;
        Vector3 startPos = trail.transform.position;

        while (alpha < 2f)
        {
            trail.transform.position = Vector3.Lerp(startPos, hitPoint, alpha);
            alpha += Time.deltaTime / 0.2f;//trailRenderer.time;


            if (alpha >= 2) ObjectPool.Instance.ReturnToPool(trail);
            yield return null;
        }
    }
}
