using UnityEngine;



public class CameraFollow : MonoBehaviour
{


    private MyCamera _camera;

    public Transform Target;

    private void Awake()
    {
        _camera = GetComponent<MyCamera>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        switch(_camera.CurrentCameraType)
        {
            case ECameraType.FirstPerson:
                transform.position = Target.position + _camera.FirstPersonOffset.PositionOffset;
                break;
            case ECameraType.ThirdPerson:
                //transform.position = Target.position + _camera.ThirdPersonOffset.PositionOffset;
                //공전해야해서 카메라 로테이션에서 같이 처리
                break;
            case ECameraType.QuarterView:
                transform.position = Target.position + _camera.QuarterViewOffset.PositionOffset;
                break;
        }

    }
}
