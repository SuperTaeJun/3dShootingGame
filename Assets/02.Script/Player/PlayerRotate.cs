using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private ECameraType _currentCameraType;

    private void Start()
    {
        MyCamera.OnCameraTypeChanged += HandleCameraTypeChanged;
    }

    private void Update()
    {
        if (_currentCameraType == ECameraType.QuarterView)
        {
            RotateTowardMouse();
        }
        else
        {
            RotateTowardCrosshair();
        }
    }
    private void HandleCameraTypeChanged(ECameraType cameraType)
    {
        _currentCameraType = cameraType;
    }
    private void RotateTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            Vector3 targetDir = hit.point - transform.position;
            targetDir.y = 0;

            if (targetDir.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(targetDir);
                transform.rotation = lookRotation;
            }
        }
    }

    private void RotateTowardCrosshair()
    {
        Vector3 forward = Camera.main.transform.forward;
        //forward.y = 0;

        if (forward != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(forward);
    }
}
