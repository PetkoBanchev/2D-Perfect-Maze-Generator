using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCellWallRemover : MonoBehaviour, IWallRemover
{
    private Cell cellType = Cell.SQUARE;
    public Cell CellType { get { return cellType; } }
    public void RemoveWalls(ICell currentCell, ICell nextCell)
    {
        int xOffset = currentCell.X - nextCell.X;
        int yOffset = currentCell.Y - nextCell.Y;

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
}
