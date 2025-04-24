using System.Collections;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField] private SO_PlayerData _playerData;
    public SO_PlayerData PlayerData => _playerData;
    float Health;
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
    public MyCamera MyCamera;
    #region Getter (Data)
    public float WalkSpeed => _playerData.WalkSpeed;
    public float RunSpeed => _playerData.RunSpeed;
    public float DashSpeed => _playerData.DashSpeed;
    public float ClimbSpeed => _playerData.ClimbSpeed;
    public float RotationSpeed => _playerData.RotationSpeed;
    public float JumpPower => _playerData.JumpPower;
    public int MaxJumpCount => _playerData.MaxJumpCount;
    public float MaxStamina => _playerData.MaxStamina;
    public float UseDashStamina => _playerData.UseDashStamina;
    public float UseRunStamina => _playerData.UseRunStamina;
    public float UseClimbStamina => _playerData.UseClimbStamina;
    #endregion

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        CurrentStamina = _playerData.MaxStamina;
        CurrentBombNum = _playerData.MaxBombNum;
        CurrentBulletNum = _playerData.MaxBulletNum;
    }

    private void Start()
    {
        Health = 100;
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
        PlayerUiManager.Instance.RefreshPlayer();
    }

    public void ReduceStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - amount, 0f, MaxStamina);
        PlayerUiManager.Instance.RefreshPlayer();
    }

    public void RecoverStamina()
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + Time.deltaTime * _playerData.RecoveryStamina, 0f, MaxStamina);
        PlayerUiManager.Instance.RefreshPlayer();
    }
    #endregion

    #region Ammo
    public void UseBomb()
    {
        CurrentBombNum--;
        PlayerUiManager.Instance.RefreshWeaponUi();
    }

    public void UseBullet()
    {
        CurrentBulletNum--;
        PlayerUiManager.Instance.RefreshWeaponUi();
    }
    #endregion

    #region Reload
    private void HandleReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            PlayerUiManager.Instance.SetActiveUI(EUiType.ReloadBar, true);
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
            PlayerUiManager.Instance.RefreshReloadBar(_reloadTimer);
            yield return null;
        }

        CurrentBulletNum = _playerData.MaxBulletNum;
        PlayerUiManager.Instance.RefreshWeaponUi();
        PlayerUiManager.Instance.SetActiveUI(EUiType.ReloadBar, false);
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
        PlayerUiManager.Instance.SetActiveUI(EUiType.ReloadBar, false);
    }
    #endregion

    private void InitUi()
    {
        PlayerUiManager.Instance.RefreshPlayer();
        PlayerUiManager.Instance.RefreshWeaponUi();
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
    }


}
