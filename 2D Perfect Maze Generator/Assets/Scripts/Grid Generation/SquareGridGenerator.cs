using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGridGenerator : MonoBehaviour, IGridGenerator
{
    #region Private Variables

    [SerializeField] GameObject cellPrefab;
    private Cell cellType = Cell.SQUARE;
    private Dictionary<Vector2, ICell> grid;

    #endregion

    #region Public Properties
    
    public Cell CellType { get { return cellType; } }

    #endregion

    #region Events
    
    public event Action<Dictionary<Vector2, ICell>> OnEmptyGridGenerated;

    #endregion

    #region Private Methods

    /// <summary>
    /// Instantiates a simple square grid using a nested for loop.
    /// The grid is stored in a 2D array. 
    /// Finally, the array is passed to the MazeManager via the OnEmptyGridGenerated event.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InstantiateGrid()
    {
        var width = MazeManager.Instance.Width;
        var height = MazeManager.Instance.Height;
        grid = new Dictionary<Vector2, ICell>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //Instantiating the cell in the scene
                Vector2 coordinates = new Vector2 (x, y);
                var cell = Instantiate(cellPrefab, coordinates, Quaternion.identity, MazeManager.Instance.MazeHolder);

                //Caching the cell into a dictionary
                grid.Add(new Vector2(x,y),cell.GetComponent<ICell>());
                grid[coordinates].Coordinates = coordinates;
            }
        }

        OnEmptyGridGenerated?.Invoke(grid);
        yield return null;
    }

    #endregion

    #region Public Methods

    public void GenerateEmptyGrid()
    {
        StartCoroutine(InstantiateGrid());
    }

    #endregion
}
