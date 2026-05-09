using System.Collections.Generic;
using UnityEngine;

public class ChunkGraphGenerator : MonoBehaviour
{
    [Header("Chunks Library")]
    public List<ChunkData> chunks;

    [Header("Generation Settings")]
    public int maxChunks = 20;
    public Vector2Int startPosition = Vector2Int.zero;
    public float chunkSize = 10f;

    private Dictionary<Vector2Int, PlacedChunk> placed = new();
    private Queue<Vector2Int> frontier = new();

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        placed.Clear();
        frontier.Clear();

        // 1. стартовый chunk
        ChunkData start = GetRandomChunk();
        PlaceChunk(start, startPosition);

        frontier.Enqueue(startPosition);

        // 2. рост графа
        while (frontier.Count > 0 && placed.Count < maxChunks)
        {
            Vector2Int currentPos = frontier.Dequeue();
            PlacedChunk current = placed[currentPos];

            TryExpand(currentPos, Vector2Int.up, current.chunk.north);
            TryExpand(currentPos, Vector2Int.right, current.chunk.east);
            TryExpand(currentPos, Vector2Int.down, current.chunk.south);
            TryExpand(currentPos, Vector2Int.left, current.chunk.west);
        }
    }

    void TryExpand(Vector2Int from, Vector2Int dir, SocketType requiredSocket)
    {
        if (requiredSocket == SocketType.None)
            return;

        Vector2Int targetPos = from + dir;

        if (placed.ContainsKey(targetPos))
            return;

        List<ChunkData> candidates = GetCompatibleChunks(Opposite(requiredSocket), dir);

        if (candidates.Count == 0)
            return;

        ChunkData selected = candidates[Random.Range(0, candidates.Count)];

        PlaceChunk(selected, targetPos);
        frontier.Enqueue(targetPos);
    }

    List<ChunkData> GetCompatibleChunks(SocketType required, Vector2Int dir)
    {
        List<ChunkData> result = new();

        foreach (var c in chunks)
        {
            if (dir == Vector2Int.up && Match(c.south, required))
                result.Add(c);

            if (dir == Vector2Int.down && Match(c.north, required))
                result.Add(c);

            if (dir == Vector2Int.left && Match(c.east, required))
                result.Add(c);

            if (dir == Vector2Int.right && Match(c.west, required))
                result.Add(c);
        }

        return result;
    }

    bool Match(SocketType a, SocketType b)
    {
        return a == b;
    }

    SocketType Opposite(SocketType s)
    {
        return s; // можно усложнить если добавишь directional sockets
    }

    ChunkData GetRandomChunk()
    {
        return chunks[Random.Range(0, chunks.Count)];
    }

    void PlaceChunk(ChunkData chunk, Vector2Int gridPos)
    {
        Vector3 worldPos = new Vector3(
            gridPos.x * chunkSize,
            0,
            gridPos.y * chunkSize
        );

        Instantiate(chunk, worldPos, Quaternion.identity, transform);

        placed.Add(gridPos, new PlacedChunk(chunk, gridPos));
    }

    void OnDrawGizmos()
    {
        if (placed == null) return;
        foreach (var kvp in placed)
        {
            Vector3 pos = new Vector3(kvp.Key.x * chunkSize, 0.5f, kvp.Key.y * chunkSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(pos, new Vector3(chunkSize, 1, chunkSize));

            // Показываем сокеты
            var c = kvp.Value.chunk;
            DrawSocket(pos, Vector3.forward, c.north, Color.blue);
            DrawSocket(pos, Vector3.right, c.east, Color.red);
            DrawSocket(pos, Vector3.back, c.south, Color.yellow);
            DrawSocket(pos, Vector3.left, c.west, Color.white);
        }
    }

    void DrawSocket(Vector3 center, Vector3 dir, SocketType type, Color color)
    {
        if (type == SocketType.None) return;
        Gizmos.color = color;
        Gizmos.DrawLine(center, center + dir * (chunkSize * 0.4f));
        Gizmos.DrawSphere(center + dir * (chunkSize * 0.4f), 0.3f);
    }
}