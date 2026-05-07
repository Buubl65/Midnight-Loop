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
    public List<WfcTile> tilePalette;

    [Header("Prefabs")]
    public GameObject cellPrefab;

    private WfcCell[,] grid;
    private List<WfcTileVariant> allPossibleVariants;

    void Start()
    {
        // 1. Сначала генерируем все варианты поворотов для всех тайлов
        allPossibleVariants = GenerateAllVariants();
        // 2. Создаем сетку
        GenerateGrid();
    }

    private List<WfcTileVariant> GenerateAllVariants()
    {
        List<WfcTileVariant> variants = new List<WfcTileVariant>();
        foreach (var tile in tilePalette)
        {
            // Для каждого тайла создаем 4 поворота (0, 90, 180, 270 градусов)
            for (int r = 0; r < 4; r++)
            {
                int[] rotatedSockets = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    // Сдвигаем сокеты в соответствии с поворотом
                    // (i - r + 4) % 4 — формула циклического сдвига массива сокетов
                    rotatedSockets[i] = tile.sockets[(i - r + 4) % 4];
                }
                variants.Add(new WfcTileVariant(tile, rotatedSockets, r * 90));
            }
        }
        return variants;
    }

    public void GenerateGrid()
    {
        grid = new WfcCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3(x * cellSize, 0, z * cellSize);
                GameObject go = Instantiate(cellPrefab, pos, Quaternion.identity, transform);

                WfcCell cell = go.GetComponent<WfcCell>();
                cell.gridPos = new Vector2Int(x, z); // Сохраняем позицию сразу!
                cell.possibleVariants = new List<WfcTileVariant>(allPossibleVariants);
                grid[x, z] = cell;
            }
        }

        RunWfc();
    }

    void RunWfc()
    {
        while (true)
        {
            WfcCell cell = GetLowestEntropyCell();
            if (cell == null) break;

            cell.Collapse();
            Propagate(cell);
        }
    }

    // Измененный Constrain для работы с WfcTileVariant
    bool Constrain(WfcCell from, WfcCell neighbor, int direction)
    {
        int oppositeDir = (direction + 2) % 4;

        // Берем все возможные сокеты со стороны "откуда пришли"
        var allowedSockets = from.possibleVariants
            .Select(v => v.sockets[direction])
            .ToHashSet();

        int before = neighbor.possibleVariants.Count;

        // Оставляем у соседа только те варианты, чей противоположный сокет подходит нам
        neighbor.possibleVariants.RemoveAll(v =>
            !allowedSockets.Contains(v.sockets[oppositeDir]));

        return neighbor.possibleVariants.Count != before;
    }

    // Теперь GetGridPos не нужен, так как у нас есть cell.gridPos!
    void Propagate(WfcCell collapsed)
    {
        Queue<WfcCell> queue = new Queue<WfcCell>();
        queue.Enqueue(collapsed);

        while (queue.Count > 0)
        {
            WfcCell current = queue.Dequeue();
            Vector2Int pos = current.gridPos;

            Vector2Int[] dirs = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

            for (int d = 0; d < 4; d++)
            {
                Vector2Int nPos = pos + dirs[d];
                if (!InBounds(nPos)) continue;

                WfcCell neighbor = grid[nPos.x, nPos.y];
                if (neighbor.isCollapsed) continue;

                if (Constrain(current, neighbor, d))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }
    }

    WfcCell GetLowestEntropyCell()
    {
        WfcCell best = null;
        int minEntropy = int.MaxValue;

        foreach (var cell in grid)
        {
            if (cell.isCollapsed || cell.Entropy <= 1) continue;
            if (cell.Entropy < minEntropy)
            {
                minEntropy = cell.Entropy;
                best = cell;
            }
        }
        return best;
    }

    bool InBounds(Vector2Int p) => p.x >= 0 && p.x < width && p.y >= 0 && p.y < height;
}