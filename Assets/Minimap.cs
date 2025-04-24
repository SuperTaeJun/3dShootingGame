using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Camera Cam;
    public Transform Target;
    public float yOffset = 15f;
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        Vector3 newPos = Target.position;
        newPos.y += yOffset;

        transform.position = newPos;
        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90;
        newEulerAngles.z = 0;
        transform.eulerAngles = newEulerAngles;
    }
    public void ZoomIn()
    {
        Cam.orthographicSize += 0.5f;
    }
    public void ZoomOut()
    {
        Cam.orthographicSize -= 0.5f;
    }
}
