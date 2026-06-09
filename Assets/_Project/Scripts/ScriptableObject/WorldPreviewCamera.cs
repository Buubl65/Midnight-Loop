using System.Collections;
using UnityEngine;

public class WorldPreviewCamera : MonoBehaviour
{
    [Header("Центр облёта")]
    public Transform target;

    [Header("Настройки")]
    public float radius = 80f;
    public float height = 120f;
    public float speed = 5f;

    private float angle;

    void Update()
    {
        if (target == null) return;

        // Увеличиваем угол
        angle += speed * Time.deltaTime;

        // Вычисляем позицию на окружности
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        Vector3 newPosition = target.position + new Vector3(x, height, z);

        // Перемещаем камеру
        transform.position = newPosition;

        // Смотрим на центр мира
        transform.LookAt(target.position);
    }
}