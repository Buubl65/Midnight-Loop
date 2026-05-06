using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public CharacterStats stats;

    void Update()
    {
        float x = InputManager.Instance.MoveX;
        float z = InputManager.Instance.MoveZ;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * stats.moveSpeed * Time.deltaTime);
    }
}
