using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [Header("Fire Settings")]
    [SerializeField] private Transform _firePos;
    [SerializeField] private GameObject _trailPrefab;
    [SerializeField] private GameObject _hitVfxPrefab;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private float _fireRange = 20f;

    [Header("Bomb Settings")]
    [SerializeField] private float _baseThrowPower = 5f;
    [SerializeField] private float _maxThrowPower = 30f;
    [SerializeField] private float _bombChargeRate = 10f;

    [Header("Scatter Settings")]
    [SerializeField] private float _scatterRadius = 2f;
    private float _distanceToSphere;

    private float _currentThrowPower;
    private Player _player;
    private Coroutine _fireCoroutine;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _currentThrowPower = _baseThrowPower;

        _distanceToSphere = _fireRange;
    }

    private void Update()
    {
        HandleBulletInput();
        HandleBombInput();
    }

    #region Bullet
    private void HandleBulletInput()
    {
        if (Input.GetMouseButtonDown(0)) StartFiring();
        if (Input.GetMouseButtonUp(0)) StopFiring();
    }

    private void StartFiring()
    {
        if (_fireCoroutine == null && _player.CurrentBulletNum > 0)
        {
            _fireCoroutine = StartCoroutine(FireLoop(_player.PlayerData.FireRate));
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
            GameObject trail = Instantiate(_trailPrefab, _firePos.position, Quaternion.identity);
            TrailRenderer trailRenderer = trail.GetComponent<TrailRenderer>();

            _player.UseBullet();
            Vector3 scatterDir = CalculateScatterDirection(_firePos.position);

            if (Physics.Raycast(_firePos.position, scatterDir, out RaycastHit hit, _fireRange))
            {
                Instantiate(_hitVfxPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                StartCoroutine(SpawnTrail(trailRenderer, hit.point));
            }
            else
            {
                Vector3 endPoint = _firePos.position + scatterDir * _fireRange;
                StartCoroutine(SpawnTrail(trailRenderer, endPoint));
            }

            yield return new WaitForSeconds(fireRate);
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint)
    {
        float alpha = 0f;
        Vector3 startPos = trail.transform.position;

        while (alpha < 2f)
        {
            trail.transform.position = Vector3.Lerp(startPos, hitPoint, alpha);
            alpha += Time.deltaTime / trail.time;
            yield return null;
        }
    }

    private Vector3 CalculateScatterDirection(Vector3 startPos)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 sphereCenter = startPos + forward * _distanceToSphere;
        Vector3 randomOffset = Random.insideUnitSphere * _scatterRadius;
        Vector3 finalPos = sphereCenter + randomOffset;
        return (finalPos - startPos).normalized;
    }
    #endregion

    #region Bomb
    private void HandleBombInput()
    {
        if (_player.CurrentBombNum <= 0) return;

        if (Input.GetMouseButtonDown(1))
            UiManager.Instance.SetActiveBombChargingBar(true);

        if (Input.GetMouseButton(1))
        {
            _currentThrowPower = Mathf.Min(_currentThrowPower + Time.deltaTime * _bombChargeRate, _maxThrowPower);
            UiManager.Instance.RefreshBombCharging(_currentThrowPower);
        }

        if (Input.GetMouseButtonUp(1))
        {
            ThrowBomb();
        }
    }

    private void ThrowBomb()
    {
        GameObject bomb = ObjectPool.Instance.GetObject(_bombPrefab);
        bomb.transform.position = _firePos.position;

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * _currentThrowPower, ForceMode.Impulse);
        rb.AddTorque(Vector3.one);

        _currentThrowPower = _baseThrowPower;
        _player.UseBomb();
        UiManager.Instance.SetActiveBombChargingBar(false);
    }
    #endregion

}
