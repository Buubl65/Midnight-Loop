using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Точки входу та виходу")]
    public bool North; // Північ
    public bool South; // Південь
    public bool East;  // Схід
    public bool West;  // Захід

    [Header("Налаштування")]
    [Range(0f, 1f)]
    public float spawnWeight = 1f; // шанс появи цього чанка

    [Header("Тип чанка")]
    public bool isBorder = false; // бордерний чанк (тупик / край карти)

    // Вспомогательный метод для быстрой проверки доступности стороны
    public bool HasExit(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return North;
        if (direction == Vector2Int.down) return South;
        if (direction == Vector2Int.right) return East;
        if (direction == Vector2Int.left) return West;
        return false;
    }

    // Возвращает список всех открытых выходов
    public System.Collections.Generic.List<Vector2Int> GetOpenExits()
    {
        var exits = new System.Collections.Generic.List<Vector2Int>();
        if (North) exits.Add(Vector2Int.up);
        if (South) exits.Add(Vector2Int.down);
        if (East) exits.Add(Vector2Int.right);
        if (West) exits.Add(Vector2Int.left);
        return exits;
    }
}
