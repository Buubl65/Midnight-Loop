using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public CharacterStats stats;

    public float gravity = 9.8f;
    public float jumpSpeed = 4.0f;
    public float terminalVelocity = 100f;

    private Vector3 velocity;

    void Update()
    {
        HandlePlayerMove();
    }

    private void HandlePlayerMove()
    {
        // Input
        float x = InputManager.Instance.MoveX;
        float z = InputManager.Instance.MoveZ;

        // Movement
        float deltaX = x * stats.moveSpeed;
        float deltaZ = z * stats.moveSpeed;

        velocity = new Vector3(deltaX, velocity.y, deltaZ);

        // Jump
        if (controller.isGrounded)
        {
            if (InputManager.Instance.JumpPressed)
            {
                velocity.y = jumpSpeed;
            }
            else
            {
                velocity.y = 0f;
            }
        }

        ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector3 moveDirection = transform.right * velocity.x +
                                transform.forward * velocity.z;

        moveDirection.y = velocity.y;

        velocity.y -= gravity * Time.deltaTime;

        velocity.y = Mathf.Max(velocity.y, -terminalVelocity);

        moveDirection.y = velocity.y;

        controller.Move(moveDirection * Time.deltaTime);
    }
}