using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] float _rotSpeed = 180f; // ī�޶�� �ӵ��� �Ȱ��ƾ���

    //0������ �����Ѵٰ� ����������
    private float _rotationX = 0;
    void Start()
    {
        
    }

    void Update()
    {
        //  ���� ����
        //  1. ���콺 �Է��� �޴´�.(���콺�� Ŀ���� ������ ����)
        float mouseX = Input.GetAxis("Mouse X");

        //  2. ���콺 �Է����κ��� ȸ����ų ������ �����.
        _rotationX += mouseX * _rotSpeed * Time.deltaTime;
        // 3. ȸ�� ��Ų��
        transform.eulerAngles = new Vector3(0, _rotationX);

    }
}
