using System;
using UnityEngine;

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
    public Player _player;

    private ECameraType _currentCameraType = ECameraType.FirstPerson;
    public ECameraType CurrentCameraType => _currentCameraType;

    [SerializeField]
    private CameraOffset _firstPersonOffset;
    [SerializeField]
    private CameraOffset _thirdPersonOffset;
    [SerializeField]
    private CameraOffset _quarterViewOffset;

    #region Getter
    public CameraOffset FirstPersonOffset => _firstPersonOffset;
    public CameraOffset ThirdPersonOffset => _thirdPersonOffset;
    public CameraOffset QuarterViewOffset => _quarterViewOffset;

    #endregion

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        CameraChangeInputHandle();
    }

    private void CameraChangeInputHandle()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ChangeCamera(ECameraType.FirstPerson);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ChangeCamera(ECameraType.ThirdPerson);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeCamera(ECameraType.QuarterView);
        }
    }

    private void ChangeCamera(ECameraType eCameraType)
    {
        _currentCameraType = eCameraType;
    }



}
