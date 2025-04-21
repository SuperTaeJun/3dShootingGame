using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] float _rotSpeed = 180f; // 카메라랑 속도가 똑같아야함

    //0도에서 시작한다고 기준을세움
    private float _rotationX = 0;
    void Start()
    {
        
    }

    void Update()
    {
        //  구현 순서
        //  1. 마우스 입력으 받는다.(마우스의 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");

        //  2. 마우스 입력으로부터 회전시킬 방향을 만든다.
        _rotationX += mouseX * _rotSpeed * Time.deltaTime;
        // 3. 회전 시킨다
        transform.eulerAngles = new Vector3(0, _rotationX);

    }
}
