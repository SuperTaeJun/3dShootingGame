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
    Recovery,
    JumpAttack
}
public enum EEnemyType
{
    Nomal,
    Chase,
    Big,
    Jump
}
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField] protected SO_EnemyData _enemyData;
    [SerializeField] private Transform _ragdollCenterBone;
    [SerializeField] private Renderer _mainRenderer;
    [SerializeField] private EEnemyType _enemyType;

    private static readonly int HitEffectStrengthID = Shader.PropertyToID("_HitEffectStrength");
    private Coroutine _hitFlashCoroutine;

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
    [Header("JumpAttackSet")]
    [SerializeField]private float _jumpAttackCoolDown = 15f;
    [SerializeField] private float _minJumAttackDistance = 4;
    public GameObject LandingZonePrefab;
    public GameObject JumpAttackVfx;
    private float lastCoolTimeJumped;
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
        _currentHealth = Data.Health;
        _uiController.SetActiveHealthBar(true);
        _uiController.RefreshPlayer(_currentHealth);


    }
    protected virtual void Awake()
    {
        _startPosition = transform.position;
        Agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _ragdolllController = GetComponent<RagdolllController>();
        _uiController = GetComponent<EnemyUiController>();
        _animator = GetComponent<Animator>();

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
        { EEnemyState.Recovery, new EnemyRecoveryState(_stateMachine, this,"Recovery") },
        { EEnemyState.JumpAttack, new EnemyJumpAttack(_stateMachine,this,"JumpAttack") }
    };

    }

    private void SetEnemyTypeForAnim()
    {
        if (_enemyType == EEnemyType.Big)
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
    public bool CanJumpAttack() => Vector3.Distance(transform.position, _player.transform.position) < Data.AttackRange*2;
    public bool CanAttack() => Vector3.Distance(transform.position, _player.transform.position) < Data.AttackRange;
    public bool CanDetect() => Vector3.Distance(transform.position, _player.transform.position) < Data.DetectRange;
    public bool CanDoJumpAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        if(distanceToPlayer < _minJumAttackDistance) return false;

        if(Time.time>lastCoolTimeJumped+_jumpAttackCoolDown)
        {
            lastCoolTimeJumped = Time.time;
            return true;
        }
        return false;
    }
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

        Vector3 origin = transform.position + Vector3.up;         
        Vector3 target = _player.transform.position + Vector3.up;  

        Vector3 toPlayer = target - origin;
        float distance = toPlayer.magnitude;

        Vector3 forward = transform.forward;
        //Y값 제거
        Vector3 flatForward = new Vector3(forward.x, 0, forward.z).normalized;
        Vector3 flatToPlayer = new Vector3(toPlayer.x, 0, toPlayer.z).normalized;

        float angle = 90f;
        float range = _enemyData.AttackRange;

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
    public void TryJumpAttack()
    {

        float radius = 4.5f;
        LayerMask playerMask = LayerMask.GetMask("Player"); // 플레이어가 이 레이어에 있어야 함


        if (JumpAttackVfx != null)
        {
            Vector3 effectPos = transform.position;
            effectPos.y += 0.05f;

            GameObject vfx = GameObject.Instantiate(JumpAttackVfx, effectPos, Quaternion.identity);
            GameObject.Destroy(vfx, 3f); 
        }


        Collider[] hits = Physics.OverlapSphere(transform.position + Vector3.up, radius, playerMask);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                Vector3 dir = (hit.transform.position - transform.position).normalized;
                damageable.TakeDamage(new Damage(_enemyData.Damage, gameObject, 20f, dir));
            }
        }
    }
    public void AcitveManualMovement(bool ManualRotation) => this.ManualMovement = ManualRotation;
    public bool ManualMovementActive() => ManualMovement;

    public void AcitveManualRotation(bool ManualRotation) => this.ManualRotation = ManualRotation;
    public bool ManualRotationActive() => ManualRotation;

    public void FlashRed(float duration = 0.15f)
    {
        if (_mainRenderer == null) return;

        if (_hitFlashCoroutine != null)
        {
            StopCoroutine(_hitFlashCoroutine);
        }

        _hitFlashCoroutine = StartCoroutine(HitFlashCoroutine(duration));
    }

    private IEnumerator HitFlashCoroutine(float duration)
    {
        Material mat = _mainRenderer.material;
        mat.SetFloat(HitEffectStrengthID, 1f);

        float timer = 0f;
        while (timer < duration)
        {
            float t = 1f - (timer / duration);
            mat.SetFloat(HitEffectStrengthID, t);
            timer += Time.deltaTime;
            yield return null;
        }

        mat.SetFloat(HitEffectStrengthID, 0f);
        _hitFlashCoroutine = null; // 코루틴 종료 후 클리어
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange);
        Gizmos.DrawWireSphere(transform.position, Data.DetectRange);

    }
}
