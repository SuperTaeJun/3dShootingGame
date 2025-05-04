using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine _stateMachine;
    protected CharacterController _characterController;
    protected Enemy _enemy;

    protected bool _triggerCalled; //나중에 애니메이션 끝났다는거 알려주는 용도로 쓸거임
    private string _animBoolName; // 애니메이션 상태변환 할때 쓸거
    protected float _stateTimer;// 각상태마다 사용할 타이머임

    public virtual void AnimFinishTrigger() => _triggerCalled = true;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName)
    {
        _stateMachine = stateMachine;
        _enemy = enemy;
        _animBoolName = animBoolName;

    }

    //나중에 애니메이션 생기면 string이랑 맞춰서 쓰셈
    protected virtual void SetAnimation(bool value)
    {
        if (!string.IsNullOrEmpty(_animBoolName))
            _enemy.Animator.SetBool(_animBoolName, value);
    }

    public virtual void Enter()
    {
        _stateTimer = 0;

        SetAnimation(true);
        _triggerCalled = false;

    }
    public virtual void Update()
    {
        _stateTimer -= Time.deltaTime;

    }
    public virtual void Exit()
    {
        SetAnimation(false);
    }

    public void AnimTrigger() => _triggerCalled = true;
}
