using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private Player _player;
    private float _rotationX;

    private MyCamera _myCamera;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _myCamera = _player.MyCamera;
    }

    private void Update()
    {
        Vector3 forward = Camera.main.transform.forward;
        //forward.y = 0;

        //화면중앙에 고정
        if (forward != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(forward);

    }
}
