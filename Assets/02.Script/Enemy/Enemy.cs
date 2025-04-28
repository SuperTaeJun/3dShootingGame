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
    Patrol
}

public class Enemy : MonoBehaviour,IDamageable
{
    [Header("Data")]
    [SerializeField] protected SO_EnemyData _enemyData;

    //넉백
    protected float knockbackPower;
    protected float knockbackDuration = 0.1f;
    //

    protected GameObject _player;
    protected EnemyUiController _uiController;
    protected EnemyStateMachine _stateMachine;
    protected CharacterController _characterController;
    protected RagdolllController _ragdolllController;
    public NavMeshAgent _agent;
    protected Animator _animator;
    protected Vector3 _startPosition;
    protected Dictionary<EEnemyState, EnemyState> _statesMap;

    protected float _currentHealth;

    #region Getter
    public GameObject Player => _player;
    public Animator Animator => _animator;
    public SO_EnemyData Data => _enemyData;
    public Vector3 StartPos => _startPosition;
    public float CurrentHealth => _currentHealth;
    #endregion

   //protected void OnEnable()
   // {
   //     _ragdolllController.DisableRagdoll();
   // }
    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _ragdolllController = GetComponent<RagdolllController>();
        _uiController = GetComponent<EnemyUiController>();
        _animator = GetComponent<Animator>();

        _agent.speed = Data.MoveSpeed;

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
        { EEnemyState.Patrol,   new EnemyPatrolState(_stateMachine, _characterController, this, "Patrol") },
    };

        _currentHealth = Data.Health;
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

    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, _player.transform.position);
    }

}
