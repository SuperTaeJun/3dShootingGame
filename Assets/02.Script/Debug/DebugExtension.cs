using UnityEngine;

public static class DebugExtension
{
    public static void DrawSphere(Vector3 position, Color color, float radius, float duration = 0f, int segments = 16)
    {
        float angleStep = 360f / segments;

        // XY 원
        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleStep * Mathf.Deg2Rad;
            float angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;
            Vector3 point1 = position + new Vector3(Mathf.Cos(angle1), Mathf.Sin(angle1), 0) * radius;
            Vector3 point2 = position + new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2), 0) * radius;
            Debug.DrawLine(point1, point2, color, duration);
        }

        // XZ 원
        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleStep * Mathf.Deg2Rad;
            float angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;
            Vector3 point1 = position + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius;
            Vector3 point2 = position + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius;
            Debug.DrawLine(point1, point2, color, duration);
        }

        // YZ 원
        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleStep * Mathf.Deg2Rad;
            float angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;
            Vector3 point1 = position + new Vector3(0, Mathf.Cos(angle1), Mathf.Sin(angle1)) * radius;
            Vector3 point2 = position + new Vector3(0, Mathf.Cos(angle2), Mathf.Sin(angle2)) * radius;
            Debug.DrawLine(point1, point2, color, duration);
        }
    }
}
