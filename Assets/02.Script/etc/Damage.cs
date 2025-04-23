using UnityEngine;

public struct Damage
{
    public Damage(int value,GameObject from, float power, Vector3 forwardDir)
    {
        Value = value;
        From = from;
        Power = power;
        ForwardDir = forwardDir;
    }
    public int Value;
    public GameObject From;
    public float Power;
    public Vector3 ForwardDir;

}
