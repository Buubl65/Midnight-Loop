using UnityEngine;

public class GTAPickup : MonoBehaviour
{
    [Header("Настройки анимации")]
    public float rotationSpeed = 150f; // Скорость вращения
    public float floatAmplitude = 0.25f; // Висота 
    public float floatSpeed = 2f; // Скорость 

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок зайшов в колайдер");
        }
    }
}