using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class CameraRotate : MonoBehaviour
{
    //카메라 회전 스크립트
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶당.
    private MyCamera _camera;
    [SerializeField] private float _rotSpeed = 300f;

    //0도에서 시작한다고 기준을세움
    private float _rotationX = 0;
    private float _rotationY = 0;

    private void Awake()
    {
        _camera = GetComponent<MyCamera>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChangeCamera();
    }
    private void ChangeCamera()
    {
        switch (_camera.CurrentCameraType)
        {
            case ECameraType.FirstPerson:
                FirstPersonRotate();
                break;
            case ECameraType.ThirdPerson:
                ThirdPersonRotate();
                break;
            case ECameraType.QuarterView:
                QuarterViewRotate();
                break;
        }
    }
    private void FirstPersonRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX += mouseX * _rotSpeed * Time.deltaTime;
        _rotationY += mouseY * _rotSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        transform.eulerAngles = new Vector3(-_rotationY, _rotationX);
    }
    private void ThirdPersonRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX += mouseX * _rotSpeed * Time.deltaTime;
        _rotationY -= mouseY * _rotSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -40f, 40f); // 위아래 제한

        // 회전 계산
        Quaternion rotation = Quaternion.Euler(_rotationY, _rotationX, 0);
        // 오프셋된 카메라 위치 더해줌
        Vector3 offset = rotation * new Vector3(0, _camera.ThirdPersonOffset.PositionOffset.y, _camera.ThirdPersonOffset.PositionOffset.z);
        //공전
        transform.position = _camera._player.transform.position + offset;

        transform.LookAt(_camera._player.transform.position + Vector3.up * _camera.ThirdPersonOffset.PositionOffset.y);
    }
    private void QuarterViewRotate()
    {
        transform.eulerAngles = _camera.QuarterViewOffset.RotationOffset;
    }
}
