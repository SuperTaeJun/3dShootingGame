using UnityEngine;

static public class CameraShake
{
    public static float shakeAmount = 3.0f;
    public static float shakeTime = 1.0f;

    public static void Shake(float ShakeAmount, float ShakeTime)
    {
        float timer = 0;
        while (timer <= ShakeTime)
        {
            Camera.main.transform.position =
                (Vector3)UnityEngine.Random.insideUnitCircle * ShakeAmount;
            timer += Time.deltaTime;
        }
        Camera.main.transform.position = new Vector3(0f, 0f, 0f);
    }
}
