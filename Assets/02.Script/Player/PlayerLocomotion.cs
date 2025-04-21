using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLocomotion : MonoBehaviour
{
    private Player player;

    private const float GRAVITY = -9.8f;
    private float _yVelocity;
    private float _currentSpeed;
    private int _jumpCount;
    private bool _isClimbing;
    private bool _isRunning;

    public bool IsRunning => _isRunning;

    private void Awake()
    {
        player = GetComponent<Player>();
        _currentSpeed = player.WalkSpeed;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);

        if (player.CharacterController.isGrounded)
        {
            _jumpCount = 0;
            if (_yVelocity < 0f)
                _yVelocity = -1f;
        }

        Jump();
        Run();
        Dash();
        Climb();

        Vector3 move = dir * _currentSpeed;
        move.y = _yVelocity;
        player.CharacterController.Move(move * Time.deltaTime);
        RecoverStamina();
    }

    private void Climb()
    {
        bool canClimb = player.IsWallInFront() && Input.GetKey(KeyCode.W) && player.CurrentStamina > 0;

        if (canClimb)
        {
            _isClimbing = true;
            player.UseStamina(player.UseClimbStamina);
            _yVelocity = player.ClimbSpeed;
        }
        else
        {
            _isClimbing = false;
            _yVelocity += GRAVITY * Time.deltaTime;
        }
    }

    private void Jump()
    {
        bool canJump = _jumpCount < player.MaxJumpCount && !_isClimbing;
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            _yVelocity = player.JumpPower;
            _jumpCount++;
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && player.CurrentStamina > 0)
        {
            _currentSpeed = player.RunSpeed;
            _isRunning = true;
            player.UseStamina(player.UseRunStamina);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _currentSpeed = player.WalkSpeed;
            _isRunning = false;
        }
    }

    private void RecoverStamina()
    {
        if (player.CurrentStamina < player.MaxStamina && !_isRunning && player.CharacterController.isGrounded)
        {
            player.RecoverStamina(10f);
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.E) && player.CurrentStamina > 0)
        {
            _currentSpeed = player.DashSpeed;
            player.ReduceStamina(player.UseDashStamina);
            StartCoroutine(FinishDash(0.2f));
        }
    }

    private IEnumerator FinishDash(float delay)
    {
        yield return new WaitForSeconds(delay);
        _currentSpeed = player.WalkSpeed;
    }
}
