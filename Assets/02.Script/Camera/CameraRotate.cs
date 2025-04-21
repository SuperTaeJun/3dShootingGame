using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    //ī�޶� ȸ�� ��ũ��Ʈ
    // ��ǥ : ���콺�� �����ϸ� ī�޶� �� �������� ȸ����Ű�� �ʹ�.

    [SerializeField] private float _rotSpeed = 300f;

    //0������ �����Ѵٰ� ����������
    private float _rotationX = 0;
    private float _rotationY = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //  ���� ����
        //  1. ���콺 �Է��� �޴´�.(���콺�� Ŀ���� ������ ����)
        float mouseX = Input.GetAxis("Mouse X"); 
        float mouseY = Input.GetAxis("Mouse Y");

        //  2. ���콺 �Է����κ��� ȸ����ų ������ �����.
        _rotationX += mouseX * _rotSpeed * Time.deltaTime;
        _rotationY += mouseY * _rotSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. ȸ�� ��Ų��
        transform.eulerAngles = new Vector3(-_rotationY, _rotationX);
    }
}
