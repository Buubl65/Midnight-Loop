using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float mouseSensitivity = 200f;
    public Transform playerBody;

    [Header("Ліміт для взору")]
    [Tooltip("Висота")]
    public float minViewAngle = -90f;
    [Tooltip("Низ")]
    public float maxViewAngle = 60f;

    float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = InputManager.Instance.MouseX * mouseSensitivity * Time.deltaTime;
        float mouseY = InputManager.Instance.MouseY * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minViewAngle, maxViewAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
