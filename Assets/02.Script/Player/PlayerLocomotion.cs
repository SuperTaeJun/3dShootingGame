using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLocomotion : MonoBehaviour
{
    private Player player;

    private const float Gravity = -9.8f;
    private float yVelocity;
    private float currentSpeed;
    private int jumpCount;
    private bool isClimbing;
    private bool isRunning;

    public bool IsRunning => isRunning;

    private void Awake()
    {
        player = GetComponent<Player>();
        currentSpeed = player.WalkSpeed;
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
            jumpCount = 0;
            if (yVelocity < 0f)
                yVelocity = -1f;
        }

        Jump();
        Run();
        Dash();
        Climb();

        Vector3 move = dir * currentSpeed;
        move.y = yVelocity;
        player.CharacterController.Move(move * Time.deltaTime);
        RecoverStamina();
    }

    private void Climb()
    {
        bool canClimb = player.IsWallInFront() && Input.GetKey(KeyCode.W) && player.CurrentStamina > 0;

        if (canClimb)
        {
            isClimbing = true;
            player.UseStamina(20f);
            yVelocity = player.ClimbSpeed;
        }
        else
        {
            isClimbing = false;
            yVelocity += Gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        bool canJump = jumpCount < player.MaxJumpCount && !isClimbing;
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            yVelocity = player.JumpPower;
            jumpCount++;
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && player.CurrentStamina > 0)
        {
            currentSpeed = player.RunSpeed;
            isRunning = true;
            player.UseStamina(4f);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = player.WalkSpeed;
            isRunning = false;
        }
    }

    private void RecoverStamina()
    {
        if (player.CurrentStamina < player.MaxStamina && !isRunning && player.CharacterController.isGrounded)
        {
            player.RecoverStamina(10f);
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.E) && player.CurrentStamina > 0)
        {
            currentSpeed = player.DashSpeed;
            player.ReduceStamina(10f);
            StartCoroutine(FinishDash(0.2f));
        }
    }

    private IEnumerator FinishDash(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentSpeed = player.WalkSpeed;
    }
}
