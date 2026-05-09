using UnityEngine;

public class PlacedChunk : MonoBehaviour
{
    public ChunkData chunk;
    public Vector2Int gridPos;

    public PlacedChunk(ChunkData chunk, Vector2Int pos)
    {
        this.chunk = chunk;
        this.gridPos = pos;
    }
}
