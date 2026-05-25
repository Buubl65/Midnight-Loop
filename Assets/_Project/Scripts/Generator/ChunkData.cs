using UnityEngine;


public enum SocketType
{
    None,
    Road,
    Wall,
    Open
}

public class ChunkData : MonoBehaviour
{
    public SocketType north;
    public SocketType east;
    public SocketType south;
    public SocketType west;

    public Vector2Int size = new Vector2Int(1, 1);
}
