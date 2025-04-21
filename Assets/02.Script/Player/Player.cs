using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 12f;
    [SerializeField] private float _dashSpeed = 50f;
    [SerializeField] private float _climbSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 180f;
    [SerializeField] private float _jumpPower = 10f;
    [SerializeField] private int _maxJumpCount = 2;

    [Header("Stamina Settings")]
    [SerializeField] private float _maxStamina = 100f;

    [Header("Wall Check Settings")]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private float _wallCheckDistance = 1.0f;
    [SerializeField] private LayerMask _wallLayer;

    public float CurrentStamina { get; private set; }
    public CharacterController CharacterController { get; private set; }

    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float DashSpeed => _dashSpeed;
    public float ClimbSpeed => _climbSpeed;
    public float RotationSpeed => _rotationSpeed;
    public float JumpPower => _jumpPower;
    public int MaxJumpCount => _maxJumpCount;
    public float MaxStamina => _maxStamina;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        CurrentStamina = _maxStamina;
    }

    private void Start()
    {
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }

    public bool IsWallInFront()
    {
        Vector3 origin = _wallCheck.position;
        Vector3 direction = _wallCheck.forward;
        bool hitWall = Physics.Raycast(origin, direction, _wallCheckDistance, _wallLayer);

        return hitWall;
    }

    public void UseStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - Time.deltaTime * amount, 0f, _maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }
    // 한번에 달게하기
    public void ReduceStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - amount, 0f, _maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }
    public void RecoverStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + Time.deltaTime * amount, 0f, _maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }
}
