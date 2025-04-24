using UnityEngine;

[CreateAssetMenu(fileName = "SO_EnemyData", menuName = "Scriptable Objects/SO_EnemyData")]
public class SO_EnemyData : ScriptableObject
{
    public float DetectRange = 7f;
    public float AttackRange = 3f;
    public float ReturnRange = 10;
    public float MoveSpeed = 2;
    public float AttackRate = 2f;
    public int   Health = 100;
    public float SturnTime = 0.5f;
    public float DeadTime = 1f;
    public float PatrolTime = 4f;
}
