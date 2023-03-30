using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGridGenerator : MonoBehaviour
{
    #region Private variables
    [SerializeField] GameObject cellPrefab;
    private Cell cellType = Cell.SQUARE;
    private ICell[,] grid;
    #endregion

    public event Action<ICell[,]> OnEmptyGridGenerated;
    public Cell CellType
    {
        get { return cellType; }
    }

    public void GenerateEmptyGrid()
    {
        StartCoroutine(InstantiateGrid());
    }

    private IEnumerator InstantiateGrid()
    {
        var width = MazeManager.Instance.Width;
        var height = MazeManager.Instance.Height;
        //var cellWidth = MazeManager.Instance.CellWidth;
        grid = new ICell[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //Instantiating the cell in the scene
                var cell = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity);
                //cell.transform.localScale = new Vector3(cellWidth, 0.1f, cellWidth);
                cell.transform.SetParent(MazeManager.Instance.MazeHolder);

                //Caching the cell into a 2D array
                grid[x, y] = cell.GetComponent<ICell>();
                grid[x, y].X = x;
                grid[x, y].Y = y;
            }
        }
        yield return null;
        OnEmptyGridGenerated?.Invoke(grid);
    }
}
