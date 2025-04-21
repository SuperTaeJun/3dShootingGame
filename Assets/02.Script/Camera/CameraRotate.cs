using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    //카메라 회전 스크립트
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶당.

    [SerializeField] private float _rotSpeed = 300f;

    //0도에서 시작한다고 기준을세움
    private float _rotationX = 0;
    private float _rotationY = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //  구현 순서
        //  1. 마우스 입력으 받는다.(마우스의 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X"); 
        float mouseY = Input.GetAxis("Mouse Y");

        //  2. 마우스 입력으로부터 회전시킬 방향을 만든다.
        _rotationX += mouseX * _rotSpeed * Time.deltaTime;
        _rotationY += mouseY * _rotSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. 회전 시킨다
        transform.eulerAngles = new Vector3(-_rotationY, _rotationX);
    }
}
