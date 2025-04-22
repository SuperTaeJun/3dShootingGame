using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private Player player;

    public Transform FirePos;
    public GameObject BombPrefab;
    public GameObject HitVfxPrefab;
    public float BaseThrowPower = 5f;
    public float CurrentThrowPower = 5f;
    public float MaxThrowPower = 30f;
    public float FireRange = 20f;

    public float DistanceToSphere = 0.5f;
    public float ScatterRadius = 2f;

    private Coroutine _fireCoroutine;


    [Header("Camera")]
    public float Roughness;
    public float Magnitude;
    public Vector3 originPos;

    [Header("Trail")]
    public GameObject TrailPrefab;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FireBomb();
        FireBullet();

        originPos = Camera.main.transform.localPosition;
    }

    private void FireBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _fireCoroutine = StartCoroutine(FireLoop(player.PlayerData.FireRate));
            player.IsFiring = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine(_fireCoroutine);
            player.IsFiring = false;
        }
    }
    private IEnumerator FireLoop(float fireRate)
    {
        while (player.CurrentBulletNum > 0)
        {
            GameObject Trail=GameObject.Instantiate(TrailPrefab);
            Trail.transform.position = FirePos.position;
            TrailRenderer trailRenderer = Trail.GetComponent<TrailRenderer>();

            player.UseBullet();

            Vector3 rotationWithScatter = TraceEndWithScatter(FirePos.position);

            Ray ray = new Ray(FirePos.position, rotationWithScatter);
            RaycastHit hit = new RaycastHit();
            Vector3 hitPoint = new Vector3();
            bool isHit = Physics.Raycast(ray, out hit, FireRange);
            if (isHit)
            {
                GameObject hitvfx = Instantiate(HitVfxPrefab);
                hitvfx.transform.position = hit.point;
                hitvfx.transform.forward = hit.normal;

                hitPoint = hit.point;
            }
            else
            {
                hitPoint = rotationWithScatter * FireRange;
            }
            StartCoroutine(SpawnTrail(trailRenderer, hitPoint));
            //디버그 시각화
            Color rayColor = isHit ? Color.red : Color.green;
            Debug.DrawRay(ray.origin, ray.direction * FireRange, rayColor, 1.0f);

            yield return new WaitForSeconds(fireRate);
        }
    }

    private void DrawDebugSphere(Vector3 center, float radius, int segments = 20)
    {
        float angleStep = 360f / segments;

        // XY 평면 원
        for (int i = 0; i < segments; i++)
        {
            float angleA = Mathf.Deg2Rad * i * angleStep;
            float angleB = Mathf.Deg2Rad * (i + 1) * angleStep;

            Vector3 pointA = center + new Vector3(Mathf.Cos(angleA), Mathf.Sin(angleA), 0) * radius;
            Vector3 pointB = center + new Vector3(Mathf.Cos(angleB), Mathf.Sin(angleB), 0) * radius;
            Debug.DrawLine(pointA, pointB, Color.green, 2.0f);
        }

        // XZ 평면 원
        for (int i = 0; i < segments; i++)
        {
            float angleA = Mathf.Deg2Rad * i * angleStep;
            float angleB = Mathf.Deg2Rad * (i + 1) * angleStep;

            Vector3 pointA = center + new Vector3(Mathf.Cos(angleA), 0, Mathf.Sin(angleA)) * radius;
            Vector3 pointB = center + new Vector3(Mathf.Cos(angleB), 0, Mathf.Sin(angleB)) * radius;
            Debug.DrawLine(pointA, pointB, Color.green, 2.0f);
        }

        // YZ 평면 원
        for (int i = 0; i < segments; i++)
        {
            float angleA = Mathf.Deg2Rad * i * angleStep;
            float angleB = Mathf.Deg2Rad * (i + 1) * angleStep;

            Vector3 pointA = center + new Vector3(0, Mathf.Cos(angleA), Mathf.Sin(angleA)) * radius;
            Vector3 pointB = center + new Vector3(0, Mathf.Cos(angleB), Mathf.Sin(angleB)) * radius;
            Debug.DrawLine(pointA, pointB, Color.green, 2.0f);
        }
    }
    private Vector3 TraceEndWithScatter(Vector3 startPos)
    {
        Vector3 DirectionToTarget = Camera.main.transform.forward;
        // 시작 위치에서 목표 방향으로 DistanceToSphere만큼 이동한 지점을 구의 중심으로 설정합니다.
        Vector3 SphereCenter = startPos + DirectionToTarget * DistanceToSphere;
        // 구의 내부에서 난수로 생성된 벡터를 계산하여 랜덤 위치를 만듭니다.
        Vector3 randomOffset = Random.insideUnitSphere * ScatterRadius;
        // 구의 중심에 난수 벡터를 추가하여 최종 위치를 계산합니다.
        Vector3 finalPos = SphereCenter + randomOffset;
        // 시작 위치에서 최종 위치까지의 벡터를 계산합니다.
        Vector3 finalDirection = (finalPos - startPos).normalized;


        // 디버그 시각화
        Debug.DrawLine(startPos, SphereCenter, Color.yellow, 2.0f);
        Debug.DrawLine(SphereCenter, finalPos, Color.red, 2.0f);
        Debug.DrawLine(startPos, startPos + finalDirection * 10.0f, Color.cyan, 2.0f);
        Debug.DrawRay(SphereCenter, Vector3.zero, Color.green, 2.0f);
        Debug.DrawRay(finalPos, Vector3.up * 0.1f, Color.magenta, 2.0f);
        DrawDebugSphere(SphereCenter, ScatterRadius);

        return finalDirection;
    }
    private void FireBomb()
    {
        if (player.CurrentBombNum > 0)
        {
            BombCharging();

            if (Input.GetMouseButtonUp(1))
            {
                GameObject bomb = ObjectPool.Instance.GetObject(BombPrefab);
                bomb.transform.position = FirePos.position;

                Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
                rigidbody.AddForce(Camera.main.transform.forward * CurrentThrowPower, ForceMode.Impulse);
                rigidbody.AddTorque(Vector3.one);

                CurrentThrowPower = BaseThrowPower;

                player.UseBomb();

                UiManager.Instance.SetActiveBombChargingBar(false);
            }
        }
    }
    private void BombCharging()
    {
        if (Input.GetMouseButtonDown(1))
            UiManager.Instance.SetActiveBombChargingBar(true);

        if (Input.GetMouseButton(1))
        {
            if (CurrentThrowPower < MaxThrowPower)
                CurrentThrowPower += Time.deltaTime * 10;

            UiManager.Instance.RefreshBombCharging(CurrentThrowPower);
        }
    }


    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint)
    {
        float alpa = 0;
        Vector3 startPos = trail.transform.position;

        while(alpa < 2)
        {
            trail.transform.position = Vector3.Lerp(startPos, hitPoint, alpa);
            alpa += Time.deltaTime / trail.time;

            yield return null;
        }
    }

}
