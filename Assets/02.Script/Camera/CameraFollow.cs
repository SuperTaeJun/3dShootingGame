using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    void Start()
    {
        
    }
    void Update()
    {
        transform.position = Target.position;
    }
}
