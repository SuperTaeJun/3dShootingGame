using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerData", menuName = "Scriptable Objects/SO_PlayerData")]
public class SO_PlayerData : ScriptableObject
{
    [Header("Player Settings")]
    public float MaxHealth = 100;
    public float WalkSpeed = 5f;
    public float RunSpeed = 12f;
    public float DashSpeed = 50f;
    public float ClimbSpeed = 2f;
    public float RotationSpeed = 180f;
    public float JumpPower = 10f;
    public int MaxJumpCount = 2;

    [Header("Stamina Settings")]
    public float MaxStamina = 100f;
    public float UseDashStamina = 10f;
    public float UseClimbStamina = 20f;
    public float UseRunStamina = 4f;
    public float RecoveryStamina = 5f;

    [Header("Weapon Settings")]
    public int MaxBombNum = 3;
    public int MaxBulletNum = 50;
    public float FireRate = 0.2f;
    public float ReloadTime = 2f;
    public int Damage = 5;
}
