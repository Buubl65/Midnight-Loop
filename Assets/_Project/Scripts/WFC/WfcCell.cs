using System.Collections.Generic;
using UnityEngine;

public class WfcCell : MonoBehaviour
{
    public bool isCollapsed = false;

    // Теперь мы храним список ВАРИАНТОВ (тайл + его поворот)
    // Этот класс WfcTileVariant мы определим ниже или в отдельном файле
    public List<WfcTileVariant> possibleVariants = new List<WfcTileVariant>();

    public int Entropy => possibleVariants.Count;

    // Координаты ячейки (полезно для оптимизации, чтобы не искать их через циклы)
    [HideInInspector] public Vector2Int gridPos;

    public void Collapse()
    {
        if (possibleVariants.Count == 0)
        {
            Debug.LogError($"Противоречие в ячейке {gridPos}: нет доступных вариантов!");
            return;
        }

        isCollapsed = true;

        WfcTileVariant selected = GetWeightedRandom();
        possibleVariants.Clear();
        possibleVariants.Add(selected);

        // Спавним объект
        GameObject go = Instantiate(selected.prefab.gameObject, transform.position, Quaternion.identity, transform);

        // ПРИМЕНЯЕМ ПОВОРОТ: вращаем вокруг оси Y
        go.transform.Rotate(0, selected.rotation, 0);
    }

    private WfcTileVariant GetWeightedRandom()
    {
        float totalWeight = 0;
        foreach (var v in possibleVariants) totalWeight += v.prefab.weight;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0;

        foreach (var v in possibleVariants)
        {
            cumulative += v.prefab.weight;
            if (roll <= cumulative) return v;
        }

        return possibleVariants[0];
    }
}