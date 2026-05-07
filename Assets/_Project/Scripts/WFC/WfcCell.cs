using System.Collections.Generic;
using UnityEngine;

public class WfcCell : MonoBehaviour
{
    public bool isCollapsed = false;
    public List<WfcTile> possibleTiles; // Список доступних варіантів

    // Повертає кількість варіантів (Ентропію)
    public int Entropy => possibleTiles.Count;

    public void Collapse()
    {
        isCollapsed = true;
        // Вибираємо один випадковий тайл із доступних
        WfcTile selectedTile = possibleTiles[Random.Range(0, possibleTiles.Count)];

        possibleTiles.Clear();
        possibleTiles.Add(selectedTile);

        // Візуалізуємо вибраний тайл
        Instantiate(selectedTile.gameObject, transform.position, transform.rotation, transform);
    }
}