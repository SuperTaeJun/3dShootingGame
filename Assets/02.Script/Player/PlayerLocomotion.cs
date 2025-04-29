using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLocomotion : MonoBehaviour
{
    private Player _player;

    private const float Gravity = -9.8f;
    private float _yVelocity;
    private float _currentSpeed;
    private int _jumpCount;
    private bool _isClimbing;
    private bool _isRunning;
    public float VerticalVelocity => _yVelocity;
    public bool IsRunning => _isRunning;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _currentSpeed = _player.WalkSpeed;
    }

    private void Update()
    {
        HandleInput();
        ApplyMovement();
        RecoveryStamina();
    }

    private void HandleInput()
    {
        Run();
        Dash();
        Jump();
        Climb();
    }

    private void ApplyMovement()
    {
        Vector3 inputDir = GetInputDirection();
        Vector3 move = inputDir * _currentSpeed;

        // 중력 적용
        if (_player.CharacterController.isGrounded && _yVelocity < 0f)
        {
            _yVelocity = -1f;
            _jumpCount = 0; // 땅에 닿았을 때 점프 카운트 초기화
        }
        else
        {
            _yVelocity += Gravity * Time.deltaTime;
        }

        move.y = _yVelocity;
        _player.CharacterController.Move(move * Time.deltaTime);
    }

    private Vector3 GetInputDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0; // 수평 이동만 반영

        return dir;
    }

    private void Climb()
    {
        bool canClimb = _player.IsWallInFront() && Input.GetKey(KeyCode.W) && _player.CurrentStamina > 0;

        if (canClimb)
        {
            _isClimbing = true;
            _player.UseStamina(_player.UseClimbStamina);
            _yVelocity = _player.ClimbSpeed;
        }
        else
        {
            _isClimbing = false;
            _yVelocity += Gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        bool canJump = _jumpCount < _player.MaxJumpCount && !_isClimbing;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            _yVelocity = _player.JumpPower;
            _jumpCount++;
        }
        //bool canJump = _jumpCount < _player.MaxJumpCount && !_isClimbing;

        //if (Input.GetKeyDown(KeyCode.Space) && canJump)
        //{
        //    _yVelocity = _player.JumpPower;
        //    _jumpCount++;
        //}

        //if (_player.CharacterController.isGrounded)
        //{
        //    _jumpCount = 0;
        //    if (_yVelocity < 0f)
        //        _yVelocity = -1f; // 살짝 눌러서 붙게 하기
        //}
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _player.CurrentStamina > 0)
        {
            _currentSpeed = _player.RunSpeed;
            _isRunning = true;
            _player.UseStamina(_player.UseRunStamina);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _currentSpeed = _player.WalkSpeed;
            _isRunning = false;
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.E) && _player.CurrentStamina > 0)
        {
            _currentSpeed = _player.DashSpeed;
            _player.ReduceStamina(_player.UseDashStamina);
            StartCoroutine(ResetSpeedAfterDelay(0.2f));
        }
    }

    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _currentSpeed = _player.WalkSpeed;
    }

    private void RecoveryStamina()
    {
        if (_player.CurrentStamina < _player.MaxStamina && !_isRunning && _player.CharacterController.isGrounded)
        {
            _player.RecoverStamina();
        }
    }
}
