using UnityEngine;

public class Rifle : MonoBehaviour
{

    private GameObject _player;
    [SerializeField] private Transform WeaponSocket;
    [SerializeField] private Vector3 weaponOffset = new Vector3(0.84f, -0.3f, 1.62f);


    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        // 카메라를 기준으로 위치와 회전을 맞춤
        transform.position = WeaponSocket.position + WeaponSocket.TransformDirection(weaponOffset);
        transform.rotation = WeaponSocket.rotation;
    }
}
