using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private ECameraType _currentCameraType;
    public float TurnSpeed = 10f; // Slerp 속도

    private void Start()
    {
        MyCamera.OnCameraTypeChanged += HandleCameraTypeChanged;
    }

    private void Update()
    {
        if (_currentCameraType == ECameraType.QuarterView)
        {
            RotateTowardMouseSmooth();
        }
        else
        {
            RotateTowardCrosshairSmooth();
        }
    }
    private void HandleCameraTypeChanged(ECameraType cameraType)
    {
        _currentCameraType = cameraType;
    }
    private void RotateTowardMouseSmooth()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            Vector3 targetDir = hit.point - transform.position;
            targetDir.y = 0;
            targetDir.Normalize();

            if (targetDir.sqrMagnitude > 0.01f)
            {
                Quaternion desiredRot = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, TurnSpeed * Time.deltaTime);
            }
        }
    }

    private void RotateTowardCrosshairSmooth()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        if (forward != Vector3.zero)
        {
            Quaternion desiredRot = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, TurnSpeed * Time.deltaTime);
        }
    }
}
