using UnityEngine;

public class ThirdPersonCameraMode
{
    private CameraOffset _offset;
    private float _rotationSpeed;
    private float _rotationX;
    private float _rotationY;

    public ThirdPersonCameraMode(CameraOffset offset, ref float rotationX, ref float rotationY, float rotationSpeed)
    {
        _offset = offset;
        _rotationX = rotationX;
        _rotationY = rotationY;
        _rotationSpeed = rotationSpeed;
    }

    public void UpdateCamera(Transform cameraTransform, Transform target)
    {
        float mouseX = Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime;

        _rotationX += mouseX;
        _rotationY = Mathf.Clamp(_rotationY - mouseY, -40f, 40f);

        Quaternion rotation = Quaternion.Euler(_rotationY, _rotationX, 0);
        Vector3 offsetPos = rotation * new Vector3(0, _offset.PositionOffset.y, _offset.PositionOffset.z);

        cameraTransform.position = target.position + offsetPos;
        cameraTransform.LookAt(target.position + Vector3.up * _offset.PositionOffset.y);
    }
}
