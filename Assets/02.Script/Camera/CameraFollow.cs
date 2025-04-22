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
                break;
            case ECameraType.QuarterView:
                transform.position = Target.position + _camera.QuarterViewOffset.PositionOffset;
                break;
        }

    }
}
