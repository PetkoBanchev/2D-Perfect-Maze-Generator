using UnityEngine;

public class SquareCellWallRemover : MonoBehaviour, IWallRemover
{
    #region Private Variables

    private Cell cellType = Cell.SQUARE;

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

        //Top Neighbour
        if (xOffset == 0 && yOffset == -1)
        {
            currentCell.RemoveWalls(Wall.TOP);
            nextCell.RemoveWalls(Wall.BOTTOM);
        }
        //Right Neighbour
        if (xOffset == -1 && yOffset == 0)
        {
            currentCell.RemoveWalls(Wall.RIGHT);
            nextCell.RemoveWalls(Wall.LEFT);
        }
        //Bottom Neighbour
        if (xOffset == 0 && yOffset == 1)
        {
            currentCell.RemoveWalls(Wall.BOTTOM);
            nextCell.RemoveWalls(Wall.TOP);
        }
        //Left Neighbour
        if (xOffset == 1 && yOffset == 0)
        {
            currentCell.RemoveWalls(Wall.LEFT);
            nextCell.RemoveWalls(Wall.RIGHT);
        }
    }

    #endregion
}
