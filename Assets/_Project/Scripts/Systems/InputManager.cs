using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public float MoveX { get; private set; }
    public float MoveZ { get; private set; }
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
    }
}
