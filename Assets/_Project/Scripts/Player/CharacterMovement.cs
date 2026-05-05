using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public CharacterStats stats;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // движение относительно направления игрока
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * stats.moveSpeed * Time.deltaTime);
    }
}
