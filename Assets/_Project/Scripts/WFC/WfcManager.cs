using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WfcManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;

    [Header("Tiles")]
    public List<WfcTile> tilePalette;   // усі доступні тайли

    [Header("Prefabs")]
    public GameObject cellPrefab;

    private WfcCell[,] grid;

    void Start() => GenerateGrid();

    public void GenerateGrid()
    {
        grid = new WfcCell[width, height];

        // 1. Ініціалізуємо клітинки
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3(x * cellSize, 0, z * cellSize);
                GameObject go = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                WfcCell cell = go.GetComponent<WfcCell>();
                cell.possibleTiles = new List<WfcTile>(tilePalette);
                grid[x, z] = cell;
            }

        // 2. Запускаємо WFC
        RunWfc();
    }

    void RunWfc()
    {
        while (true)
        {
            WfcCell cell = GetLowestEntropyCell();
            if (cell == null) break;        // усі колапсовані

            cell.Collapse();
            Propagate(cell);
        }
    }

    WfcCell GetLowestEntropyCell()
    {
        WfcCell best = null;
        int minEntropy = int.MaxValue;

        foreach (var cell in grid)
        {
            if (cell.isCollapsed) continue;
            if (cell.Entropy < minEntropy)
            {
                minEntropy = cell.Entropy;
                best = cell;
            }
        }
        return best;
    }

    void Propagate(WfcCell collapsed)
    {
        Queue<WfcCell> queue = new Queue<WfcCell>();
        queue.Enqueue(collapsed);

        while (queue.Count > 0)
        {
            WfcCell current = queue.Dequeue();
            Vector2Int pos = GetGridPos(current);

            // Сусіди: Північ, Схід, Південь, Захід
            Vector2Int[] dirs = {
                Vector2Int.up, Vector2Int.right,
                Vector2Int.down, Vector2Int.left
            };

            for (int d = 0; d < 4; d++)
            {
                Vector2Int nPos = pos + dirs[d];
                if (!InBounds(nPos)) continue;

                WfcCell neighbor = grid[nPos.x, nPos.y];
                if (neighbor.isCollapsed) continue;

                bool changed = Constrain(current, neighbor, d);
                if (changed) queue.Enqueue(neighbor);
            }
        }
    }

    // Повертає true якщо список сусіда змінився
    bool Constrain(WfcCell from, WfcCell neighbor, int direction)
    {
        int oppositeDir = (direction + 2) % 4;
        var allowedSockets = from.possibleTiles
            .Select(t => t.sockets[direction])
            .ToHashSet();

        int before = neighbor.possibleTiles.Count;
        neighbor.possibleTiles.RemoveAll(t =>
            !allowedSockets.Contains(t.sockets[oppositeDir]));

        return neighbor.possibleTiles.Count != before;
    }

    Vector2Int GetGridPos(WfcCell cell)
    {
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
                if (grid[x, z] == cell) return new Vector2Int(x, z);
        return Vector2Int.zero;
    }

    bool InBounds(Vector2Int p) =>
        p.x >= 0 && p.x < width && p.y >= 0 && p.y < height;
}