using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject _player;
    public GameObject Player => _player;

    protected EnemyStateMachine _stateMachine;
    protected CharacterController _characterController;
    protected Animator _animator;
    public Animator Animator => _animator;

    public Transform _startPosition;

    [Header("Stat")]
    public float DetectRange = 7f;
    public float AttackRange = 3f;
    public float ReturnRange = 10;
    private float _moveSpeed = 2;
    public float MoveSpeed => _moveSpeed;

    #region Staties
    public EnemyIdleState IdleState;
    public EnemyTraceState TraceState;
    public EnemyReturnState ReturnState;
    public EnemyDamagedState DamagedState;
    public EnemyAttackState AttackState;
    public EnemyDeadState DeadState;
    #endregion

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _stateMachine = new EnemyStateMachine();

        _startPosition = transform;


        IdleState = new EnemyIdleState(_stateMachine, _characterController, this, "Idle");
        TraceState = new EnemyTraceState(_stateMachine, _characterController, this, "Trace");
        ReturnState = new EnemyReturnState(_stateMachine, _characterController, this, "Retrun", _startPosition);
        DamagedState = new EnemyDamagedState(_stateMachine, _characterController, this, "Damaged");
        AttackState = new EnemyAttackState(_stateMachine, _characterController, this, "Attack");
        DeadState = new EnemyDeadState(_stateMachine, _characterController, this, "Dead");

    }
    void Start()
    {
        _stateMachine.InitStateMachine(IdleState,this);
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        _stateMachine.Update();
    }
}
