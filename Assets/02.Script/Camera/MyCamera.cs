using System;
using UnityEngine;
using static UnityEditor.SceneView;

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
}

public class MyCamera : MonoBehaviour
{
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

    public static event Action<ECameraType> OnCameraTypeChanged;
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
                break;
            case ECameraType.ThirdPerson:
                _thirdPerson.UpdateCamera(transform, _target);
                break;
            case ECameraType.QuarterView:
                _quarterView.UpdateCamera(transform, _target);
                break;
        }
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

        if (Input.GetKeyDown(KeyCode.F1)) Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.F2)) Cursor.lockState = CursorLockMode.None;
    }

    private void SetCameraType(ECameraType newType)
    {
        _currentCameraType = newType;
        OnCameraTypeChanged?.Invoke(newType); // 이벤트 발행
    }
}
