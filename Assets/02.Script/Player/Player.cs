using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
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
    public bool _isReloading = false;

    private float _reloadTimer = 0;
    private Coroutine _reloadCoroutine;

    #region Getter
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
        UiManager.Instance.RefreshStamina(CurrentStamina);
        UiManager.Instance.RefreshBomb(CurrentBombNum, _playerData._MaxBombNum);
        UiManager.Instance.RefreshBullet(CurrentBulletNum, _playerData._MaxBulletNum);
    }
    private void Update()
    {
        Reload();
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
        CurrentStamina = Mathf.Clamp(CurrentStamina - Time.deltaTime * amount, 0f, _playerData._maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }
    // 한번에 달게하기
    public void ReduceStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina - amount, 0f, _playerData._maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }
    public void RecoverStamina()
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + Time.deltaTime * _playerData._recoveryStamina, 0f, _playerData._maxStamina);
        UiManager.Instance.RefreshStamina(CurrentStamina);
    }

    public void UseBomb()
    {
        CurrentBombNum -= 1;
        UiManager.Instance.RefreshBomb(CurrentBombNum, _playerData._MaxBombNum);
    }
    public void UseBullet()
    {
        CurrentBulletNum -= 1;
        UiManager.Instance.RefreshBullet(CurrentBulletNum, _playerData._MaxBulletNum);
    }


    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            UiManager.Instance.SetActiveReloadBar(true);
            _reloadCoroutine = StartCoroutine(ReloadStart(_playerData.ReloadTime));
        }
    }

    private IEnumerator ReloadStart(float reloadTime)
    {
        _isReloading = true;
        _reloadTimer = 0f;

        while (_reloadTimer < reloadTime)
        {
            // 만약 사격이 발생하면 중단
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
        UiManager.Instance.RefreshBullet(CurrentBulletNum, _playerData._MaxBulletNum);
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
}
