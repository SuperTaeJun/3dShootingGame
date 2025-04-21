using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 목표 : wasd를 누르면 캐릭터을 이동시키고 싶당.

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpPower = 10f;

    private const float GRAVITY = -9.8f;
    private float _yVelocity = 0f;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        //구현순서
        // 1. 키보드 입력을 받늗나.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        // 2. 입력으로부터 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);

        // 3. 점프
        if(Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = _jumpPower;
        }
        


        // 4. 중력 적용
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        // 5. 방향에 따라 플레이어를 이동한다
        _characterController.Move(dir * _moveSpeed * Time.deltaTime);

    }
}
