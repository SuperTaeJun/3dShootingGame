using UnityEngine;

public class FirstPersonCameraMode 
{
    private Vector3 _positionOffset;
    private float _rotationSpeed;
    private float _rotationX;
    private float _rotationY;

    public FirstPersonCameraMode(Vector3 positionOffset, float rotationSpeed)
    {
        _positionOffset = positionOffset;
        _rotationSpeed = rotationSpeed;
    }

    public void UpdateCamera(Transform cameraTransform, Transform target)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX += mouseX * _rotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY + mouseY * _rotationSpeed * Time.deltaTime, -60f, 60f);

        cameraTransform.position = target.position + _positionOffset;
        cameraTransform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
    }
}
