using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
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

public class Enemy : MonoBehaviour
{
    private GameObject _player;
    public GameObject Player => _player;

    protected EnemyStateMachine _stateMachine;
    protected CharacterController _characterController;
    protected Animator _animator;
    public Animator Animator => _animator;

    private Vector3 _startPosition;
    public Vector3 StartPos => _startPosition;
    [Header("Stat")]
    public float DetectRange = 7f;
    public float AttackRange = 3f;
    public float ReturnRange = 10;
    public float _moveSpeed = 2;
    public float MoveSpeed => _moveSpeed;
    public float AttackRate = 2f;
    public int Health = 100;
    public float SturnTime = 0.5f;
    public float DeadTime = 1f;
    public float PatrolTime = 4f;


    [Header("Knockback")]
    public float knockbackPower = 10f;
    public float knockbackDuration = 0.3f;
    private Vector3 knockbackDirection;
    private float knockbackTimer;
    private bool isKnockbacking = false;

    private Dictionary<EEnemyState, EnemyState> _statesMap;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _stateMachine = new EnemyStateMachine();

        _startPosition = transform.position;

        _statesMap = new Dictionary<EEnemyState, EnemyState>
    {
        { EEnemyState.Idle, new EnemyIdleState(_stateMachine, _characterController, this, "Idle") },
        { EEnemyState.Trace, new EnemyTraceState(_stateMachine, _characterController, this, "Trace") },
        { EEnemyState.Return, new EnemyReturnState(_stateMachine, _characterController, this, "Return", _startPosition) },
        { EEnemyState.Damaged, new EnemyDamagedState(_stateMachine, _characterController, this, "Damaged") },
        { EEnemyState.Attack, new EnemyAttackState(_stateMachine, _characterController, this, "Attack") },
        { EEnemyState.Dead, new EnemyDeadState(_stateMachine, _characterController, this, "Dead") },
        { EEnemyState.Patrol, new EnemyPatrolState(_stateMachine, _characterController, this, "Patrol") },
    };

    }
    void Start()
    {
        _stateMachine.InitStateMachine(_statesMap[EEnemyState.Idle], this, _statesMap);
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        _stateMachine.Update();
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        knockbackPower = damage.Power;
        //_characterController.Move(-transform.forward * damage.Power);
        StartCoroutine(KnockbackCoroutine(damage.ForwardDir));
        _stateMachine.ChangeState(EEnemyState.Damaged);
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction)
    {
        isKnockbacking = true;
        float timer = 0f;

        Vector3 knockbackDir = direction.normalized;

        while (timer < knockbackDuration)
        {
            _characterController.Move(knockbackDir * knockbackPower * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isKnockbacking = false;
    }
}
