using System;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] protected SO_WeaponData _data;

    [SerializeField] protected Transform _attackPos;
    [SerializeField] protected GameObject _hitVfxPrefab;

    protected Player _player;
    protected RaycastHit _traceHitResult;
    protected ECameraType _currentCameraType;
    public SO_WeaponData Data => _data;

    public static Action OnTriggerFireStart;
    public static Action OnTriggerFireEnd;

    public abstract void Attack();
    protected virtual void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        MyCamera.OnCameraTypeChanged += HandleCameraTypeChanged;
    }
    protected virtual void Update()
    {
        TraceUnderCrosshair(out _traceHitResult, _data.AttackRange);
    }

    // 화면중앙으로 레이저를쏴서 헤어라인에 맞는 녀석을 구함 없으면 공격사거리 최대에 맞았다고침
    protected virtual void TraceUnderCrosshair(out RaycastHit hitInfo, float distance)
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
            //Debug.DrawLine(start, hitInfo.point, Color.red, 1f);
            return;
        }
        else
        {
            hitInfo.point = end;
            //Debug.DrawLine(start, end, Color.green, 1f);
            return;
        }
    }
    protected void HandleCameraTypeChanged(ECameraType type)
    {
        _currentCameraType = type;
    }

}
