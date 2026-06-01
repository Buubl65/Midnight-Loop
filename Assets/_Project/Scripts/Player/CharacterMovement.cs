using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public CharacterStats stats;

    [Header("Physics")]
    public float gravity = 9.8f;
    public float terminalVelocity = 100f;

    private Vector3 velocity;

    [Header("Dash")]
    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector3 dashDirection;

    void Update()
    {
        HandleTimers();

        if (isDashing)
        {
            HandleDash();
            return;
        }

        HandlePlayerMove();
        HandleDashInput();
    }

    private void HandlePlayerMove()
    {
        float x = InputManager.Instance.MoveX;
        float z = InputManager.Instance.MoveZ;

        velocity.x = x * stats.moveSpeed;
        velocity.z = z * stats.moveSpeed;

        if (controller.isGrounded)
        {
            if (InputManager.Instance.JumpPressed &&
                stats.HasEnoughStamina(stats.jumpStaminaCost))
            {
                stats.UseStamina(stats.jumpStaminaCost);

                velocity.y = stats.jumpSpeed;
            }
            else if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }

        ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector3 moveDirection = (transform.right * InputManager.Instance.MoveX +
                                 transform.forward * InputManager.Instance.MoveZ).normalized;

        Vector3 horizontalVelocity = moveDirection * stats.moveSpeed;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        velocity.y -= gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, -terminalVelocity);

        Vector3 totalMovement = (horizontalVelocity + Vector3.up * velocity.y) * Time.deltaTime;
        controller.Move(totalMovement);
    }

    private void HandleDashInput()
    {
        if (InputManager.Instance.DashPressed &&
        dashCooldownTimer <= 0f &&
        stats.HasEnoughStamina(stats.dashStaminaCost))
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        stats.UseStamina(stats.dashStaminaCost);

        isDashing = true;

        dashTimer = stats.dashDuration;
        dashCooldownTimer = stats.dashCooldown;

        float x = InputManager.Instance.MoveX;
        float z = InputManager.Instance.MoveZ;

        dashDirection =
            (transform.right * x +
             transform.forward * z).normalized;

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }
    }

    private void HandleDash()
    {
        controller.Move(
            dashDirection *
            stats.dashSpeed *
            Time.deltaTime);

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            isDashing = false;
        }
    }

    private void HandleTimers()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }
}