using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float climbSpeed = 2f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private int maxJumpCount = 2;

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;

    [Header("Wall Check Settings")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 1.0f;
    [SerializeField] private LayerMask wallLayer;

    public float CurrentStamina { get; private set; }
    public CharacterController CharacterController { get; private set; }

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float DashSpeed => dashSpeed;
    public float ClimbSpeed => climbSpeed;
    public float RotationSpeed => rotationSpeed;
    public float JumpPower => jumpPower;
    public int MaxJumpCount => maxJumpCount;
    public float MaxStamina => maxStamina;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        CurrentStamina = maxStamina;
    }

    private void Start()
    {
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }

    public bool IsWallInFront()
    {
        Vector3 origin = wallCheck.position;
        Vector3 direction = wallCheck.forward;
        bool hitWall = Physics.Raycast(origin, direction, out _, wallCheckDistance, wallLayer);

        Debug.DrawRay(origin, direction * wallCheckDistance, hitWall ? Color.red : Color.green);
        return hitWall;
    }

    public void UseStamina(float amountPerSecond)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - Time.deltaTime * amountPerSecond, 0f, maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }

    public void ReduceStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - amount, 0f, maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }

    public void RecoverStamina(float amountPerSecond)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + Time.deltaTime * amountPerSecond, 0f, maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }
}
