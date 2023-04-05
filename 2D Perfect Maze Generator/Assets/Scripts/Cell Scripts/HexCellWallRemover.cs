using UnityEngine;

public class HexCellWallRemover : MonoBehaviour, IWallRemover
{
    #region Private Variables

    private Cell cellType = Cell.HEXAGON;

    #endregion

    #region Public Properties
    public Cell CellType { get { return cellType; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Removes the walls between two neighbouring cells. 
    /// The position of the neighbour is determined by calculating the offset of the cells' coordinates.
    /// Then the respective walls are removed.
    /// </summary>
    /// <param name="currentCell"></param>
    /// <param name="nextCell"></param>
    public void RemoveWalls(ICell currentCell, ICell nextCell)
    {
        int xOffset = (int)(currentCell.Coordinates.x - nextCell.Coordinates.x);
        int yOffset = (int)(currentCell.Coordinates.y - nextCell.Coordinates.y);

        //Top Right Neighbour (x + 1, y + 1)
        if (xOffset == -1 && yOffset == -1)
        {
            currentCell.RemoveWalls(Wall.TOP_RIGHT);
            nextCell.RemoveWalls(Wall.BOTTOM_LEFT);
        }
        //Right Neigbhour (x + 2, y)
        if (xOffset == -2 && yOffset == 0)
        {
            currentCell.RemoveWalls(Wall.RIGHT);
            nextCell.RemoveWalls(Wall.LEFT);
        }
        //Bottom Right Neighbour (x + 1, y - 1)
        if (xOffset == -1 && yOffset == 1)
        {
            currentCell.RemoveWalls(Wall.BOTTOM_RIGHT);
            nextCell.RemoveWalls(Wall.TOP_LEFT);
        }
        //Bottom Left Neighbour (x - 1, y - 1)
        if (xOffset == 1 && yOffset == 1)
        {
            currentCell.RemoveWalls(Wall.BOTTOM_LEFT);
            nextCell.RemoveWalls(Wall.TOP_RIGHT);
        }
        //Left Neigbhour (x - 2, y)
        if (xOffset == 2 && yOffset == 0)
        {
            currentCell.RemoveWalls(Wall.LEFT);
            nextCell.RemoveWalls(Wall.RIGHT);
        }
        //Top Left Neighbour (x - 1, y + 1)
        if (xOffset == 1 && yOffset == -1)
        {
            currentCell.RemoveWalls(Wall.TOP_LEFT);
            nextCell.RemoveWalls(Wall.BOTTOM_RIGHT);
        }
    }

    #endregion
}
