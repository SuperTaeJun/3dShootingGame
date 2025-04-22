using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private SO_PlayerData _playerData;
    public SO_PlayerData PlayerData => _playerData;

    [Header("Wall Check Settings")]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private float _wallCheckDistance = 1.0f;
    [SerializeField] private LayerMask _wallLayer;

    public float CurrentStamina { get; private set; }
    public int CurrentBombNum { get; private set; }
    public int CurrentBulletNum { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public bool IsFiring = false;
    private bool _isReloading = false;
    private float _reloadTimer = 0f;
    private Coroutine _reloadCoroutine;
    public MyCamera Camera;
    #region Getter (Data)
    public float WalkSpeed => _playerData._walkSpeed;
    public float RunSpeed => _playerData._runSpeed;
    public float DashSpeed => _playerData._dashSpeed;
    public float ClimbSpeed => _playerData._climbSpeed;
    public float RotationSpeed => _playerData._rotationSpeed;
    public float JumpPower => _playerData._jumpPower;
    public int MaxJumpCount => _playerData._maxJumpCount;
    public float MaxStamina => _playerData._maxStamina;
    public float UseDashStamina => _playerData._useDashStamina;
    public float UseRunStamina => _playerData._useRunStamina;
    public float UseClimbStamina => _playerData._useClimbStamina;
    #endregion

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        CurrentStamina = _playerData._maxStamina;
        CurrentBombNum = _playerData._MaxBombNum;
        CurrentBulletNum = _playerData._MaxBulletNum;
    }

    private void Start()
    {
        InitUi();
    }

    private void Update()
    {
        HandleReloadInput();
    }

    public bool IsWallInFront()
    {
        return Physics.Raycast(_wallCheck.position, _wallCheck.forward, _wallCheckDistance, _wallLayer);
    }

    #region Stamina
    public void UseStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - Time.deltaTime * amount, 0f, MaxStamina);
        UpdateStaminaUI();
    }

    public void ReduceStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - amount, 0f, MaxStamina);
        UpdateStaminaUI();
    }

    public void RecoverStamina()
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + Time.deltaTime * _playerData._recoveryStamina, 0f, MaxStamina);
        UpdateStaminaUI();
    }
    #endregion

    #region Ammo
    public void UseBomb()
    {
        CurrentBombNum--;
        UpdateBombUI();
    }

    public void UseBullet()
    {
        CurrentBulletNum--;
        UpdateBulletUI();
    }
    #endregion

    #region Reload
    private void HandleReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            UiManager.Instance.SetActiveReloadBar(true);
            _reloadCoroutine = StartCoroutine(ReloadRoutine(_playerData.ReloadTime));
        }
    }

    private IEnumerator ReloadRoutine(float reloadTime)
    {
        _isReloading = true;
        _reloadTimer = 0f;

        while (_reloadTimer < reloadTime)
        {
            if (IsFiring)
            {
                CancelReload();
                yield break;
            }

            _reloadTimer += Time.deltaTime;
            UiManager.Instance.RefreshReloadBar(_reloadTimer);
            yield return null;
        }

        CurrentBulletNum = _playerData._MaxBulletNum;
        UpdateBulletUI();
        UiManager.Instance.SetActiveReloadBar(false);
        _reloadTimer = 0f;
        _isReloading = false;
    }

    private void CancelReload()
    {
        if (_reloadCoroutine != null)
        {
            StopCoroutine(_reloadCoroutine);
            _reloadCoroutine = null;
        }

        _isReloading = false;
        _reloadTimer = 0f;
        UiManager.Instance.SetActiveReloadBar(false);
    }
    #endregion

    #region UI
    private void InitUi()
    {
        UpdateStaminaUI();
        UpdateBombUI();
        UpdateBulletUI();
    }

    private void UpdateStaminaUI()
    {
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }

    private void UpdateBombUI()
    {
        UiManager.Instance.RefreshBomb(CurrentBombNum, _playerData._MaxBombNum);
    }

    private void UpdateBulletUI()
    {
        UiManager.Instance.RefreshBullet(CurrentBulletNum, _playerData._MaxBulletNum);
    }
    #endregion
}
