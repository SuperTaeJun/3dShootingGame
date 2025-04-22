using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private Player _player;
    private float _rotationX;

    private MyCamera _myCamera;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _myCamera = _player.Camera;
    }

    private void Update()
    {
        if (_myCamera == null || _myCamera.CurrentCameraType == ECameraType.QuarterView)
            return;


        float mouseX = Input.GetAxis("Mouse X");
        _rotationX += mouseX * _player.RotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, _rotationX, 0);
    }
}
