using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerData", menuName = "Scriptable Objects/SO_PlayerData")]
public class SO_PlayerData : ScriptableObject
{
    [Header("Movement Settings")]
    public float _walkSpeed = 5f;
    public float _runSpeed = 12f;
    public float _dashSpeed = 50f;
    public float _climbSpeed = 2f;
    public float _rotationSpeed = 180f;
    public float _jumpPower = 10f;
    public int _maxJumpCount = 2;

    [Header("Stamina Settings")]
    public float _maxStamina = 100f;
    public float _useDashStamina = 10f;
    public float _useClimbStamina = 20f;
    public float _useRunStamina = 4f;
    public float _recoveryStamina = 5f;
}
