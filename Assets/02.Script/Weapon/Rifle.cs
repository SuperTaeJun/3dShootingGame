using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 weaponOffset = new Vector3(0.84f, -0.3f, 1.62f);

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // 카메라를 기준으로 위치와 회전을 맞춤
        transform.position = cameraTransform.position + cameraTransform.TransformDirection(weaponOffset);
        transform.rotation = cameraTransform.rotation;
    }
}
