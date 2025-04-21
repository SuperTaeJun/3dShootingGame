using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private Player player;
    private float rotationX;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        rotationX += mouseX * player.RotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, rotationX, 0);
    }
}
