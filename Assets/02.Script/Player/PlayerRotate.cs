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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 lookPos = hit.point;
            //lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }
        else
        {
            Vector3 lookDir = ray.direction;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}
