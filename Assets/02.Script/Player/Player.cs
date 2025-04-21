using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [SerializeField] private SO_PlayerData _PlayerData;


    [Header("Wall Check Settings")]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private float _wallCheckDistance = 1.0f;
    [SerializeField] private LayerMask _wallLayer;

    public float CurrentStamina { get; private set; }
    public CharacterController CharacterController { get; private set; }

    #region Getter
    public float WalkSpeed => _PlayerData._walkSpeed;
    public float RunSpeed => _PlayerData._runSpeed;
    public float DashSpeed => _PlayerData._dashSpeed;
    public float ClimbSpeed => _PlayerData._climbSpeed;
    public float RotationSpeed => _PlayerData._rotationSpeed;
    public float JumpPower => _PlayerData._jumpPower;
    public int MaxJumpCount => _PlayerData._maxJumpCount;
    public float MaxStamina => _PlayerData._maxStamina;
    public float UseDashStamina => _PlayerData._useDashStamina;
    public float UseRunStamina => _PlayerData._useRunStamina;
    public float UseClimbStamina => _PlayerData._useClimbStamina;
#endregion

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        CurrentStamina = _PlayerData._maxStamina;
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
        CurrentStamina = Mathf.Clamp(CurrentStamina - Time.deltaTime * amount, 0f, _PlayerData._maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }
    // 한번에 달게하기
    public void ReduceStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - amount, 0f, _PlayerData._maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }
    public void RecoverStamina()
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + Time.deltaTime * _PlayerData._recoveryStamina, 0f, _PlayerData._maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }
}
