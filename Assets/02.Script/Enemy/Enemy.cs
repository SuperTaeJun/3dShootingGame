using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EEnemyState
{
    Idle,
    Trace,
    Return,
    Damaged,
    Attack,
    Dead,
    Move,
    Recovery
}
public enum EEnemyType
{
    Nomal,
    Chase,
    Big
}
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField] protected SO_EnemyData _enemyData;
    [SerializeField] private Transform _ragdollCenterBone;
    [SerializeField] private Renderer _mainRenderer;
    [SerializeField] private EEnemyType _enemyType;
    private Color _originalColor;
    public Transform RagdollCenterBone => _ragdollCenterBone;


    public GameObject CurrencyDropPrefab;

    protected GameObject _player;
    protected EnemyUiController _uiController;
    public EnemyUiController UiController => _uiController;
    protected EnemyStateMachine _stateMachine;
    protected CharacterController _characterController;
    protected RagdolllController _ragdolllController;
    public NavMeshAgent Agent;
    protected Animator _animator;
    protected Vector3 _startPosition;
    protected Dictionary<EEnemyState, EnemyState> _statesMap;

    protected int _currentHealth;

    private bool ManualRotation;
    private bool ManualMovement;

    #region Getter
    public GameObject Player => _player;
    public Animator Animator => _animator;
    public SO_EnemyData Data => _enemyData;
    public Vector3 StartPos => _startPosition;
    public int CurrentHealth => _currentHealth;
    public EEnemyType EnemyType => _enemyType;
    #endregion

    protected void OnEnable()
    {
        _startPosition = transform.position;
        Debug.Log(_startPosition);
        _currentHealth = Data.Health;
        _uiController.SetActiveHealthBar(true);
        _uiController.RefreshPlayer(_currentHealth);
        _mainRenderer.material.color = _originalColor;
    }
    protected virtual void Awake()
    {
        _startPosition = transform.position;
        Agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _ragdolllController = GetComponent<RagdolllController>();
        _uiController = GetComponent<EnemyUiController>();
        _animator = GetComponent<Animator>();

 

        _originalColor = _mainRenderer.material.color;
        Agent.speed = Data.MoveSpeed;

        _stateMachine = new EnemyStateMachine();

        _statesMap = new Dictionary<EEnemyState, EnemyState>
    {
        { EEnemyState.Idle,     new EnemyIdleState(_stateMachine, this, "Idle") },
        { EEnemyState.Trace,    new EnemyTraceState(_stateMachine, this, "Trace") },
        { EEnemyState.Return,   new EnemyReturnState(_stateMachine, this, "Return", _startPosition) },
        { EEnemyState.Damaged,  new EnemyDamagedState(_stateMachine, this, "Damaged") },
        { EEnemyState.Attack,   new EnemyAttackState(_stateMachine, this, "Attack") },
        { EEnemyState.Dead,     new EnemyDeadState(_stateMachine, this, "Idle",_ragdolllController) },
        { EEnemyState.Move,   new EnemyMoveState(_stateMachine, this, "Move") },
        { EEnemyState.Recovery, new EnemyRecoveryState(_stateMachine, this,"Recovery") }
    };

    }

    private void SetEnemyTypeForAnim()
    {
        if(_enemyType == EEnemyType.Big)
            _animator.SetFloat("EnemyType", 1.0f);
        else
            _animator.SetFloat("EnemyType", 0.0f);
    }

    protected virtual void Start()
    {
        SetEnemyTypeForAnim();
        _stateMachine.InitStateMachine(_statesMap[EEnemyState.Idle], this, _statesMap);
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
    {
        _stateMachine.Update();
    }

    public void TakeDamage(Damage damage)
    {
        _currentHealth -= damage.Value;
        _uiController.RefreshPlayer(_currentHealth);

        _stateMachine.ChangeState(EEnemyState.Damaged);

    }

    public void AnimTrigger() => _stateMachine.CurrentState.AnimTrigger();
    public bool CanAttack() => Vector3.Distance(transform.position, _player.transform.position) < Data.AttackRange;
    public bool CanDetect() => Vector3.Distance(transform.position, _player.transform.position) < Data.DetectRange;

    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, _player.transform.position);
    }

    public Quaternion ForwardTarget(Vector3 Target)
    {
        Quaternion TargetRot = Quaternion.LookRotation(Target - transform.position);

        Vector3 CurEulerAngles = transform.rotation.eulerAngles;

        float yRot = Mathf.LerpAngle(CurEulerAngles.y, TargetRot.eulerAngles.y, _enemyData.TrunSpeed * Time.deltaTime);

        return Quaternion.Euler(CurEulerAngles.x, yRot, CurEulerAngles.z);
    }
    public void TryAttack()
    {
        if (_player == null) return;

        Vector3 origin = transform.position + Vector3.up;           // 적 시야 기준 위치
        Vector3 target = _player.transform.position + Vector3.up;   // 플레이어 기준 위치

        Vector3 toPlayer = target - origin;
        float distance = toPlayer.magnitude;

        Vector3 forward = transform.forward;
        //Y값 제거
        Vector3 flatForward = new Vector3(forward.x, 0, forward.z).normalized;
        Vector3 flatToPlayer = new Vector3(toPlayer.x, 0, toPlayer.z).normalized;

        float angle = 45f;
        float range = _enemyData.AttackRange;

        // === 충돌 체크 ===
        if (distance <= range)
        {
            float angleToPlayer = Vector3.Angle(flatForward, flatToPlayer);
            if (angleToPlayer <= angle * 0.5f)
            {
                if (_player.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    damageable.TakeDamage(new Damage(_enemyData.Damage, gameObject, 20f, flatForward));
                }
            }
        }
    }

    public void AcitveManualMovement(bool ManualRotation) => this.ManualMovement = ManualRotation;
    public bool ManualMovementActive() => ManualMovement;

    public void AcitveManualRotation(bool ManualRotation) => this.ManualRotation = ManualRotation;
    public bool ManualRotationActive() => ManualRotation;



    public void FlashRed(float duration = 0.1f)
    {
        if (_mainRenderer == null) return;
        StopAllCoroutines(); // 중복 방지
        StartCoroutine(FlashCoroutine(duration));
    }

    private IEnumerator FlashCoroutine(float duration)
    {
        _mainRenderer.material.color = Color.red;
        yield return new WaitForSeconds(duration);
        _mainRenderer.material.color = _originalColor;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, Data.AttackRange);
    //    Gizmos.DrawWireSphere(transform.position, Data.DetectRange);
    //}
}
