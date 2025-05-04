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

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField] protected SO_EnemyData _enemyData;
    [SerializeField] private Transform _ragdollCenterBone;
    public Transform RagdollCenterBone => _ragdollCenterBone;


    public GameObject CurrencyDropPrefab;

    //넉백
    protected float knockbackPower;
    protected float knockbackDuration = 0.1f;
    //

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

    #region Getter
    public GameObject Player => _player;
    public Animator Animator => _animator;
    public SO_EnemyData Data => _enemyData;
    public Vector3 StartPos => _startPosition;
    public int CurrentHealth => _currentHealth;
    #endregion

    protected void OnEnable()
    {
        //_ragdolllController.DisableRagdoll();
        _currentHealth = Data.Health;
        _uiController.SetActiveHealthBar(true);
        _uiController.RefreshPlayer(_currentHealth);
    }
    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _ragdolllController = GetComponent<RagdolllController>();
        _uiController = GetComponent<EnemyUiController>();
        _animator = GetComponent<Animator>();

        Agent.speed = Data.MoveSpeed;

        _stateMachine = new EnemyStateMachine();

        _startPosition = transform.position;

        _statesMap = new Dictionary<EEnemyState, EnemyState>
    {
        { EEnemyState.Idle,     new EnemyIdleState(_stateMachine, _characterController, this, "Idle") },
        { EEnemyState.Trace,    new EnemyTraceState(_stateMachine, _characterController, this, "Trace") },
        { EEnemyState.Return,   new EnemyReturnState(_stateMachine, _characterController, this, "Return", _startPosition) },
        { EEnemyState.Damaged,  new EnemyDamagedState(_stateMachine, _characterController, this, "Damaged") },
        { EEnemyState.Attack,   new EnemyAttackState(_stateMachine, _characterController, this, "Attack") },
        { EEnemyState.Dead,     new EnemyDeadState(_stateMachine, _characterController, this, "Dead",_ragdolllController) },
        { EEnemyState.Move,   new EnemyMoveState(_stateMachine, _characterController, this, "Move") },
        { EEnemyState.Recovery, new EnemyRecoveryState(_stateMachine, _characterController, this,"Recovery") }
    };

    }

    protected virtual void Start()
    {
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
        knockbackPower = damage.Power;

        _uiController.RefreshPlayer(_currentHealth);

        StartCoroutine(KnockbackCoroutine(damage.ForwardDir));
        _stateMachine.ChangeState(EEnemyState.Damaged);

    }

    private IEnumerator KnockbackCoroutine(Vector3 direction)
    {
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            _characterController.Move(direction * knockbackPower * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
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


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange);
        Gizmos.DrawWireSphere(transform.position, Data.DetectRange);
    }
}
