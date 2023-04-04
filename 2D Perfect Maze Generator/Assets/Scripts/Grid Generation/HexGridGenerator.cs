using System;
using System.Collections;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour, IGridGenerator
{
    #region Private Variables
    [SerializeField] GameObject cellPrefab;
    //hexgon parameters
    [SerializeField] private float hexRadius;
    private float hexWidth;
    private float hexHeight;
    private Cell cellType = Cell.HEXAGON;
    private ICell[,] grid;
    #endregion

    #region Public Variables
    public event Action<ICell[,]> OnEmptyGridGenerated;
    #endregion

    #region Public Properties
    public Cell CellType
    {
        get { return cellType; }
    }
    #endregion

    #region Public methods
    public void GenerateEmptyGrid()
    {
        CalculateHexWidthAndHeight();
        StartCoroutine(InstantiateGrid());
    }
    #endregion

    #region Private methods
    /// <summary>
    /// Calculate the width and the height of the hexagon based on the radius.
    /// For more information check https://www.redblobgames.com/grids/hexagons/#map-storage
    /// </summary>
    private void CalculateHexWidthAndHeight()
    {
        hexWidth = hexRadius * Mathf.Sqrt(3); // Sqrt(3) come from Sin(60)
        hexHeight = hexRadius * 2;
    }

    /// <summary>
    /// Calculates the world position based on the tile's coordinates within the hex grid
    /// Because we use doubled coordinates there is no need to calculate additional offsets for even and odd rows
    /// xPos is divided by 2 because we use double coordinates
    /// Each row is offset by 3/4 of the hexHeight
    /// </summary>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <returns></returns>
    private Vector2 CalculateWorldPosition(float xPos, float yPos)
    {
        float x = (xPos / 2) * hexWidth;
        float y = yPos * hexHeight * 0.75f;

        return new Vector2(x, y);
    }
    /// <summary>
    /// Generates a hexagonal based on doubled coordinates.
    /// An event is fired at the end to pass the grid along.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InstantiateGrid()
    {
        var width = MazeManager.Instance.Width;
        var height = MazeManager.Instance.Height;
        grid = new ICell[width, height];

        for (int y = 0; y < MazeManager.Instance.Height; y++)
        {
            for (int x = 0; x < MazeManager.Instance.Width; x++)
            {
                // This check ensures we use double coorditates
                // Example (x: 2, z: 4) or (x: 1, z: 3); NEVER (x: 2, z: 3)
                // For more info check https://www.redblobgames.com/grids/hexagons/#map-storage
                if ((x % 2 == 0 && y % 2 == 0) || (x % 2 != 0 && y % 2 != 0))
                {
                    var cell = Instantiate(cellPrefab, CalculateWorldPosition(x, y), Quaternion.identity, MazeManager.Instance.MazeHolder);

                    //Caching the cell into a 2D array
                    grid[x, y] = cell.GetComponent<ICell>();
                    grid[x, y].X = x;
                    grid[x, y].Y = y;

                }
            }
        }
        yield return null;
        OnEmptyGridGenerated?.Invoke(grid);
    }
    #endregion
}
