using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject ChargingParticle;


    [Header("Scatter Settings")]
    [SerializeField] private float _scatterRadius = 2f;
    private float _distanceToSphere;

    private float _currentThrowPower;
    private Player _player;
    private Coroutine _fireCoroutine;

    private RaycastHit _traceHitResult;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _currentThrowPower = _baseThrowPower;

        _distanceToSphere = _fireRange;
    }

    private void Update()
    {
        //크로스헤어에서 발사체가 날아가는 방향으로 레이캐스트를 하여 무엇을 조준 중인지 확인
        TraceUnderCrosshair(out _traceHitResult, _fireRange);

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
            GameObject trail = ObjectPool.Instance.GetObject(_trailPrefab);
            trail.transform.position = _firePos.position;
            trail.transform.rotation = gameObject.transform.rotation;

            _player.UseBullet();

            // 총기 반동 계산 해서 방향 결정
            Vector3 scatterDir = CalculateScatterDirection(_firePos.position);

            if (Physics.Raycast(_firePos.position, scatterDir, out RaycastHit hit, _fireRange))
            {
                Debug.DrawLine(_firePos.position, hit.point, Color.red, 2f);
                if (hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    enemy.TakeDamage(new Damage(_player.PlayerData.Damage, _player.gameObject, 20f,transform.forward));
                }
                else if(hit.collider.CompareTag("Prop"))
                {
                    if (hit.collider.gameObject != null)
                    {

                        Drum drum = hit.collider.GetComponent<Drum>();
                        drum.TakeDamage(new Damage(_player.PlayerData.Damage, _player.gameObject, 20f, transform.forward));
                    }
                }
                Instantiate(_hitVfxPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                StartCoroutine(SpawnTrail(trail, hit.point));
            }
            else
            {
                Vector3 endPoint = _firePos.position + scatterDir * _fireRange;
                Debug.DrawLine(_firePos.position, endPoint, Color.red, 2f);
                StartCoroutine(SpawnTrail(trail, endPoint));
            }

            yield return new WaitForSeconds(fireRate);
        }
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

    private Vector3 CalculateScatterDirection(Vector3 startPos)
    {
        //정규화
        Vector3 forward = (_traceHitResult.point - startPos).normalized;

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
        {
            PlayerUiManager.Instance.SetActiveUI(EUiType.BombChargingBar, true);
            ChargingParticle.SetActive(true);
            ChargingParticle.transform.position = _firePos.position;
        }

        if (Input.GetMouseButton(1))
        {
            _currentThrowPower = Mathf.Min(_currentThrowPower + Time.deltaTime * _bombChargeRate, _maxThrowPower);
            PlayerUiManager.Instance.RefreshBombCharging(_currentThrowPower);
        }

        if (Input.GetMouseButtonUp(1))
        {
            ChargingParticle.SetActive(false);
            ThrowBomb();
        }
    }

    private void ThrowBomb()
    {
        Bomb bomb = ObjectPool.Instance.GetObject(_bombPrefab).GetComponent<Bomb>();
        bomb.transform.position = _firePos.position;
        bomb.transform.rotation = _player.transform.rotation;

        Vector3 scatterDir = CalculateScatterDirection(_firePos.position);
        bomb.Initialize(scatterDir, _currentThrowPower, _player.gameObject);

        _currentThrowPower = _baseThrowPower;
        _player.UseBomb();
        PlayerUiManager.Instance.SetActiveUI(EUiType.BombChargingBar, false);
    }
    #endregion



    public void TraceUnderCrosshair(out RaycastHit hitInfo, float distance)
    {
        Camera cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        //화면 중앙에서 나가는 Ray
        Ray ray = cam.ScreenPointToRay(screenCenter);

        //시작 지점 → 카메라 위치와 캐릭터 거리만큼 앞으로 보정
        Vector3 start = ray.origin;
        float distToCharacter = Vector3.Distance(start, transform.position);
        start += ray.direction * (distToCharacter + 1.0f);

        //최종 레이캐스트
        Vector3 end = start + ray.direction * distance;

        if (Physics.Raycast(start, ray.direction, out hitInfo, distance))
        {
            Debug.DrawLine(start, hitInfo.point, Color.red, 1f);
            return;
        }
        else
        {
            hitInfo.point = end;
            Debug.DrawLine(start, end, Color.green, 1f);
            return;
        }
    }
}
