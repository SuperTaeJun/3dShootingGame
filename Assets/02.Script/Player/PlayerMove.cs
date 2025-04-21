using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // ��ǥ : wasd�� ������ ĳ������ �̵���Ű�� �ʹ�.

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
        //��������
        // 1. Ű���� �Է��� �ވf��.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        // 2. �Է����κ��� ������ �����Ѵ�.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);

        // 3. ����
        if(Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = _jumpPower;
        }
        


        // 4. �߷� ����
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        // 5. ���⿡ ���� �÷��̾ �̵��Ѵ�
        _characterController.Move(dir * _moveSpeed * Time.deltaTime);

    }
}
