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

        WfcTile selected = GetWeightedRandom();
        possibleTiles.Clear();
        possibleTiles.Add(selected);

        Instantiate(selected.gameObject, transform.position, transform.rotation, transform);
    }

    private WfcTile GetWeightedRandom()
    {
        int totalWeight = 0;
        foreach (var t in possibleTiles) totalWeight += t.weight;

        int roll = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var t in possibleTiles)
        {
            cumulative += t.weight;
            if (roll < cumulative) return t;
        }

        return possibleTiles[0]; // fallback
    }
}