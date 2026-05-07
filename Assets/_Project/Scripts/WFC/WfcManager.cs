using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WfcManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public WfcTile[] tilePrefabs; 

    private WfcCell[,] grid;
    public GameObject cellPrefab;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        grid = new WfcCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cellObj = Instantiate(cellPrefab, new Vector3(x, 0, y), Quaternion.identity);
                WfcCell cell = cellObj.GetComponent<WfcCell>();
                cell.possibleTiles = new List<WfcTile>(tilePrefabs);
                grid[x, y] = cell;
            }
        }
    }
}