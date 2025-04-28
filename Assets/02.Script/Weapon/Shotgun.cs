using UnityEngine;
using System.Collections;
public class Shotgun : WeaponBase
{
    [Header("Fire Settings")]
    [SerializeField] private GameObject _trailPrefab;
    [SerializeField] private int _numOfPill;

    [Header("Scatter Settings")]
    [SerializeField] private float _scatterRadius = 2f;
    private float _distanceToSphere;

    private Coroutine _fireCoroutine;

    protected override void Start()
    {
        base.Start();
        _distanceToSphere = _attackRange;
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


                GameObject trail = ObjectPool.Instance.GetObject(_trailPrefab);
                trail.transform.position = _attackPos.position;
                trail.transform.rotation = gameObject.transform.rotation;

                _player.UseBullet();

                Vector3 fireDir = GetFireDirection();

                if (Physics.Raycast(_attackPos.position, fireDir, out RaycastHit hit, _attackRange))
                {
                    Debug.DrawLine(_attackPos.position, hit.point, Color.red, 2f);

                    if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                    {
                        damageable.TakeDamage(new Damage(_data.Damage, _player.gameObject, 20f, transform.forward));
                    }

                    Instantiate(_hitVfxPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    StartCoroutine(SpawnTrail(trail, hit.point));
                }
                else
                {
                    Vector3 endPoint = _attackPos.position + fireDir * _attackRange;
                    Debug.DrawLine(_attackPos.position, endPoint, Color.red, 2f);
                    StartCoroutine(SpawnTrail(trail, endPoint));
                }
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
                Vector3 dir = (targetPos - _attackPos.position).normalized;
                return dir;
            }
            return _attackPos.forward; // fallback
        }
        else
        {
            TraceUnderCrosshair(out _traceHitResult, _attackRange);
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
