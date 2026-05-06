using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public CharacterStats stats;
    public float gravity = -9.81f;

    private Vector3 velocity;

    void Update()
    {
        float x = InputManager.Instance.MoveX;
        float z = InputManager.Instance.MoveZ;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * stats.moveSpeed * Time.deltaTime);
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
