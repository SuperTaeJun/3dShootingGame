using System;
using UnityEngine;
using System.Collections;
using Unity.Mathematics;
public enum ECameraType
{
    FirstPerson,
    ThirdPerson,
    QuarterView
}
[Serializable]
public struct CameraOffset
{
    public Vector3 PositionOffset;
    public Vector3 RotationOffset;
    public float FieldOfView;
}

public class MyCamera : MonoBehaviour
{
    // 이제 Camera 컴포넌트를 캐싱해 둡니다
    private Camera _camera;

    [SerializeField] private Transform _target;
    [SerializeField] private CameraOffset _firstPersonOffset;
    [SerializeField] private CameraOffset _thirdPersonOffset;
    [SerializeField] private CameraOffset _quarterViewOffset;
    [SerializeField] private float _rotationSpeed = 300f;

    private ECameraType _currentCameraType = ECameraType.FirstPerson;
    public ECameraType CurrentCameraType => _currentCameraType;

    private FirstPersonCameraMode _firstPerson;
    private ThirdPersonCameraMode _thirdPerson;
    private QuarterViewCameraMode _quarterView;

    private float _rotationX;
    private float _rotationY;


    // 카메라 쉐이크용
    private Vector3 _shakeOffset = Vector3.zero;
    private Coroutine _shakeCoroutine;

    public static event Action<ECameraType> OnCameraTypeChanged;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _shakeOffset = Vector3.zero;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _firstPerson = new FirstPersonCameraMode(_firstPersonOffset.PositionOffset, _rotationSpeed);
        _thirdPerson = new ThirdPersonCameraMode(_thirdPersonOffset, ref _rotationX, ref _rotationY, _rotationSpeed);
        _quarterView = new QuarterViewCameraMode(_quarterViewOffset);
    }

    private void Update()
    {
        HandleInput();

        switch (_currentCameraType)
        {
            case ECameraType.FirstPerson:
                _firstPerson.UpdateCamera(transform, _target);
                _camera.fieldOfView = _firstPersonOffset.FieldOfView;
                break;
            case ECameraType.ThirdPerson:
                _thirdPerson.UpdateCamera(transform, _target);
                _camera.fieldOfView = _thirdPersonOffset.FieldOfView;
                break;
            case ECameraType.QuarterView:
                _quarterView.UpdateCamera(transform, _target);
                _camera.fieldOfView = _quarterViewOffset.FieldOfView;
                break;
        }

        // 쉐이크 오프셋 적용
        transform.position += _shakeOffset;
    }
    public void Shake(float duration, float magnitude)
    {
        // 이미 진행 중이면 멈추고 새로 시작
        if (_shakeCoroutine != null)
            StopCoroutine(_shakeCoroutine);
        _shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalOffset = Vector3.zero;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 매 프레임 랜덤 오프셋
            _shakeOffset = UnityEngine.Random.insideUnitSphere * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 끝나면 초기화
        _shakeOffset = Vector3.zero;
        _shakeCoroutine = null;
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8)) 
        {
            SetCameraType(ECameraType.FirstPerson);
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetCameraType(ECameraType.ThirdPerson);
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetCameraType(ECameraType.QuarterView);
            Cursor.lockState = CursorLockMode.None;
        }

        //if (Input.GetKeyDown(KeyCode.F1)) Cursor.lockState = CursorLockMode.Locked;
        //if (Input.GetKeyDown(KeyCode.F2)) Cursor.lockState = CursorLockMode.None;
    }

    private void SetCameraType(ECameraType newType)
    {
        _currentCameraType = newType;
        OnCameraTypeChanged?.Invoke(newType); // 이벤트 발행
    }
}
