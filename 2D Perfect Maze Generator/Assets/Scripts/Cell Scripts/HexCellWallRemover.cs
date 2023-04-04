using UnityEngine;

public class HexCellWallRemover : MonoBehaviour, IWallRemover
{
    private Cell cellType = Cell.HEXAGON;
    public Cell CellType { get { return cellType; } }
    public void RemoveWalls(ICell currentCell, ICell nextCell)
    {
        int xOffset = currentCell.X - nextCell.X;
        int yOffset = currentCell.Y - nextCell.Y;

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
}
