using UnityEngine;

public class WfcTile : MonoBehaviour
{
    public enum TileType { Road, Building, Empty, Prop }


    [Header("Sockets")]
    public int[] sockets = new int[4];

    [Header("Info")]
    public TileType tileType;
    public int weight = 1;

    [Header("Placement")]
    public bool canPlaceProp;
}
