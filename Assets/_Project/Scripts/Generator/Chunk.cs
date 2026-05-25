using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Точки входу та виходу")]
    public bool North; // Північ (+Z)
    public bool South; // Південь (-Z)
    public bool East;  // Схід (+X)
    public bool West;  // Захід (-X)

    [Header("Налаштування")]
    [Range(0f, 1f)]
    public float spawnWeight = 1f;

    [Header("Тип чанка")]
    public bool isBorder = false;

    public bool HasExit(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return North;
        if (direction == Vector2Int.down) return South;
        if (direction == Vector2Int.right) return East;
        if (direction == Vector2Int.left) return West;
        return false;
    }

    public System.Collections.Generic.List<Vector2Int> GetOpenExits()
    {
        var exits = new System.Collections.Generic.List<Vector2Int>();
        if (North) exits.Add(Vector2Int.up);
        if (South) exits.Add(Vector2Int.down);
        if (East) exits.Add(Vector2Int.right);
        if (West) exits.Add(Vector2Int.left);
        return exits;
    }

    // ВІЗУАЛІЗАЦІЯ ДЛЯ НАЛАШТУВАННЯ
    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + Vector3.up * 0.5f; // піднімаємо лінії трохи над землею
        float lineLength = 4f; // довжина стрілочки (налаштуйте під розмір чанка)

        // Перевіряємо локальні напрямки з урахуванням поточного повороту об'єкта
        if (North)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(pos, transform.forward * lineLength);
        }
        if (South)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(pos, -transform.forward * lineLength);
        }
        if (East)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(pos, transform.right * lineLength);
        }
        if (West)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(pos, -transform.right * lineLength);
        }

        // Малюємо маленьку сферу в центрі чанка
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(pos, 0.3f);
    }
}