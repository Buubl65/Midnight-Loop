using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCGenerator : MonoBehaviour
{
    [System.Serializable]
    public class WFCTile
    {
        public Chunk prefab;
        public int rotationIndex; // 0 = 0°, 1 = 90°, 2 = 180°, 3 = 270°

        public bool North;
        public bool South;
        public bool East;
        public bool West;

        public float weight;
        public bool isBorder;

        public WFCTile(Chunk prefab, int rotationIndex)
        {
            this.prefab = prefab;
            this.rotationIndex = rotationIndex;
            this.weight = prefab.spawnWeight;
            this.isBorder = prefab.isBorder;

            switch (rotationIndex)
            {
                case 0:
                    North = prefab.North; East = prefab.East; South = prefab.South; West = prefab.West;
                    break;
                case 1: // 90 градусів
                    North = prefab.West; East = prefab.North; South = prefab.East; West = prefab.South;
                    break;
                case 2: // 180 градусів
                    North = prefab.South; East = prefab.West; South = prefab.North; West = prefab.East;
                    break;
                case 3: // 270 градусів
                    North = prefab.East; East = prefab.South; South = prefab.West; West = prefab.North;
                    break;
            }
        }
    }

    private class Cell
    {
        public Vector2Int position;
        public List<WFCTile> options = new List<WFCTile>();
        public bool isCollapsed = false;
        public WFCTile collapsedTile = null;

        public Cell(Vector2Int pos, List<WFCTile> allPossibleTiles)
        {
            position = pos;
            options = new List<WFCTile>(allPossibleTiles);
        }
    }

    [Header("Налаштування генерації")]
    [SerializeField] private List<Chunk> chunkPrefabs;
    [SerializeField] private Vector2Int gridSize = new Vector2Int(10, 10);
    [SerializeField] private float chunkSize = 10f;
    [SerializeField] private Transform player;
    [SerializeField] private int maxGenerationAttempts = 50; // Максимальна кількість спроб перезапуску

    private Cell[,] grid;
    private List<WFCTile> allTilesPrototype = new List<WFCTile>();
    private readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

    private void Start()
    {
        GenerateMap();
    }

    // ОНОВЛЕНА ФУНКЦІЯ З ЦИКЛОМ ПЕРЕЗАПУСКУ
    public void GenerateMap()
    {
        int currentAttempt = 0;
        bool generationSuccess = false;

        while (currentAttempt < maxGenerationAttempts)
        {
            currentAttempt++;

            PrepareTilesPrototypes();
            InitializeGrid();

            if (RunWFC())
            {
                generationSuccess = true;
                break; // Генерація пройшла успішно, виходимо з циклу спроб!
            }

            // Якщо зайшли в глухий кут, цикл піде на наступне коло, скинувши grid
        }

        if (generationSuccess)
        {
            SpawnPlayerInCenter();
            InstantiateGrid();
            Debug.Log($"WFC: Мапу успішно згенеровано з правильними з'єднаннями за {currentAttempt} спроб(и)!");
        }
        else
        {
            Debug.LogError($"WFC: Навіть за {maxGenerationAttempts} спроб алгоритм уперся в суперечність. Твоїм тайлам фізично не вистачає комбінацій/поворотів, щоб закрити мапу!");
        }
    }

    private void PrepareTilesPrototypes()
    {
        allTilesPrototype.Clear();
        foreach (var prefab in chunkPrefabs)
        {
            if (prefab == null) continue;
            for (int r = 0; r < 4; r++)
            {
                allTilesPrototype.Add(new WFCTile(prefab, r));
            }
        }
    }

    private void InitializeGrid()
    {
        grid = new Cell[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                grid[x, y] = new Cell(new Vector2Int(x, y), allTilesPrototype);
                ApplyBorderConstraints(grid[x, y]);
            }
        }
    }

    private void ApplyBorderConstraints(Cell cell)
    {
        cell.options.RemoveAll(tile =>
            (cell.position.y == gridSize.y - 1 && tile.North) ||
            (cell.position.y == 0 && tile.South) ||
            (cell.position.x == gridSize.x - 1 && tile.East) ||
            (cell.position.x == 0 && tile.West)
        );

        bool isEdge = cell.position.x == 0 || cell.position.x == gridSize.x - 1 ||
                     cell.position.y == 0 || cell.position.y == gridSize.y - 1;

        if (isEdge && cell.options.Exists(t => t.isBorder))
        {
            cell.options.RemoveAll(t => !t.isBorder);
        }
    }

    private bool RunWFC()
    {
        while (true)
        {
            Cell nextCell = null;
            int minOptions = int.MaxValue;
            bool allCollapsed = true;

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Cell cell = grid[x, y];
                    if (cell.isCollapsed) continue;

                    allCollapsed = false;
                    int count = cell.options.Count;

                    if (count == 0) return false; // Повертаємо false при глухому куті

                    if (count < minOptions)
                    {
                        minOptions = count;
                        nextCell = cell;
                    }
                }
            }

            if (allCollapsed) return true;
            if (nextCell == null) return false;

            CollapseCell(nextCell);

            if (!PropagateConstraints(nextCell))
            {
                return false; // Повертаємо false, якщо обмеження призвели до 0 варіантів
            }
        }
    }

    private void CollapseCell(Cell cell)
    {
        if (cell.options.Count == 0) return;

        WFCTile selectedTile = GetWeightedRandomTile(cell.options);
        cell.options.Clear();
        cell.options.Add(selectedTile);
        cell.collapsedTile = selectedTile;
        cell.isCollapsed = true;
    }

    private WFCTile GetWeightedRandomTile(List<WFCTile> options)
    {
        float totalWeight = 0;
        foreach (var tile in options) totalWeight += tile.weight;

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeightSum = 0;

        foreach (var tile in options)
        {
            currentWeightSum += tile.weight;
            if (randomValue <= currentWeightSum) return tile;
        }
        return options[0];
    }

    private bool PropagateConstraints(Cell collapsedCell)
    {
        Queue<Cell> queue = new Queue<Cell>();
        queue.Enqueue(collapsedCell);

        while (queue.Count > 0)
        {
            Cell current = queue.Dequeue();

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighborPos = current.position + dir;
                if (!IsValidPosition(neighborPos)) continue;

                Cell neighbor = grid[neighborPos.x, neighborPos.y];
                if (neighbor.isCollapsed) continue;

                int previousOptionsCount = neighbor.options.Count;

                for (int i = neighbor.options.Count - 1; i >= 0; i--)
                {
                    WFCTile neighborTile = neighbor.options[i];
                    bool hasMatch = false;

                    foreach (WFCTile currentTile in current.options)
                    {
                        if (CheckCompatibility(currentTile, neighborTile, dir))
                        {
                            hasMatch = true;
                            break;
                        }
                    }

                    if (!hasMatch)
                    {
                        neighbor.options.RemoveAt(i);
                    }
                }

                if (neighbor.options.Count != previousOptionsCount)
                {
                    if (neighbor.options.Count == 0) return false;
                    queue.Enqueue(neighbor);
                }
            }
        }
        return true;
    }

    private bool CheckCompatibility(WFCTile current, WFCTile neighbor, Vector2Int direction)
    {
        if (direction == Vector2Int.up) return current.North == neighbor.South;
        if (direction == Vector2Int.down) return current.South == neighbor.North;
        if (direction == Vector2Int.right) return current.East == neighbor.West;
        if (direction == Vector2Int.left) return current.West == neighbor.East;
        return false;
    }

    private bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize.x && pos.y >= 0 && pos.y < gridSize.y;
    }

    private void InstantiateGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Cell cell = grid[x, y];
                if (cell.collapsedTile != null)
                {
                    Vector3 spawnPos = new Vector3(x * chunkSize, 0, y * chunkSize);
                    Vector3 originalRot = cell.collapsedTile.prefab.transform.eulerAngles;

                    float newYRot = originalRot.y + (cell.collapsedTile.rotationIndex * 90f);
                    Quaternion spawnRot = Quaternion.Euler(originalRot.x, newYRot, originalRot.z);

                    Chunk spawnedChunk = Instantiate(cell.collapsedTile.prefab, spawnPos, spawnRot, transform);
                    spawnedChunk.name = $"Chunk_{x}_{y}_Rot_{cell.collapsedTile.rotationIndex * 90}";

                    ChunkEnemySpawner spawner = spawnedChunk.GetComponentInChildren<ChunkEnemySpawner>();
                    if (spawner != null)
                    {
                        spawner.SpawnEnemies();
                    }
                }
            }
        }
    }

    private void SpawnPlayerInCenter()
    {
        int centerX = gridSize.x / 2;
        int centerY = gridSize.y / 2;

        Vector3 spawnPos = new Vector3(
            centerX * chunkSize,
            2f,
            centerY * chunkSize
        );

        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
            cc.enabled = false;

        player.position = spawnPos;

        if (cc != null)
            cc.enabled = true;
    }
}