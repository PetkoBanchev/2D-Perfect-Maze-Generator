using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour, ICell
{
    #region Private Variables
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private bool isVisited = false;
    [SerializeField] private GameObject[] wallObjects;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private List<ICell> neighbours = new List<ICell>();
    private bool areNeighboursCached = false;
    #endregion

    #region Private methods
    private void CacheUnvisitedNeighbours()
    {
        //Top Right Neighbour (x + 1, y + 1)
        if (x + 1 < MazeManager.Instance.Width && y + 1 < MazeManager.Instance.Height)
        {
            var neigbhour = MazeManager.Instance.GetCell(x + 1, y + 1);
            if (!neigbhour.IsVisited)
                neighbours.Add(neigbhour);
        }
        //Right Neigbhour (x + 2, y)
        if (x + 2 < MazeManager.Instance.Width)
        {
            var neigbhour = MazeManager.Instance.GetCell(x + 2, y);
            if (!neigbhour.IsVisited)
                neighbours.Add(neigbhour);
        }
        //Bottom Right Neighbour (x + 1, y - 1)
        if (x + 1 < MazeManager.Instance.Width && y - 1 >= 0)
        {
            var neigbhour = MazeManager.Instance.GetCell(x + 1, y - 1);
            if (!neigbhour.IsVisited)
                neighbours.Add(neigbhour);
        }
        //Bottom Left Neighbour (x - 1, y - 1)
        if (x - 1 >= 0 && y - 1 >= 0)
        {
            var neigbhour = MazeManager.Instance.GetCell(x - 1, y - 1);
            if (!neigbhour.IsVisited)
                neighbours.Add(neigbhour);
        }
        //Left Neigbhour (x - 2, y)
        if (x - 2 >= 0)
        {
            var neigbhour = MazeManager.Instance.GetCell(x - 2, y);
            if (!neigbhour.IsVisited)
                neighbours.Add(neigbhour);
        }
        //Top Left Neighbour (x - 1, y + 1)
        if (x - 1 >= 0 && y + 1 < MazeManager.Instance.Height)
        {
            var neigbhour = MazeManager.Instance.GetCell(x - 1, y + 1);
            if (!neigbhour.IsVisited)
                neighbours.Add(neigbhour);
        }

        areNeighboursCached = true;
    }
    #endregion

    #region Public properties
    public int X
    {
        get { return x; }
        set { x = value; }
    }
    public int Y
    {
        get { return y; }
        set { y = value; }
    }

    public bool IsVisited
    {
        get { return isVisited; }
        set { isVisited = value; }
    }
    #endregion

    #region Public methods
    public void SetColor(Color color)
    {
        spriteRenderer.material.color = color;
    }

    public void RemoveWalls(Wall wall)
    {
        switch (wall)
        {
            case Wall.TOP_RIGHT:
                wallObjects[0].SetActive(false);
                break;
            case Wall.RIGHT:
                wallObjects[1].SetActive(false);
                break;
            case Wall.BOTTOM_RIGHT:
                wallObjects[2].SetActive(false);
                break;
            case Wall.BOTTOM_LEFT:
                wallObjects[3].SetActive(false);
                break;
            case Wall.LEFT:
                wallObjects[4].SetActive(false);
                break;
            case Wall.TOP_LEFT:
                wallObjects[5].SetActive(false);
                break;
        }
    }
    public ICell GetRandomUnvisitedNeighbour()
    {
        if (!areNeighboursCached)
            CacheUnvisitedNeighbours();

        if (neighbours.Count > 0)
        {
            RemoveVisitedNeighbours();
            // Checks if any unvisited neighbours remain. If there are none it returns null
            if (neighbours.Count <= 0)
                return null;

            // Return a random neigbhour. It is safe to not check if the randomNeighbour has been visited, since all visited neighbours are removed above.
            int randomIndex = Random.Range(0, neighbours.Count);
            return neighbours[randomIndex];

        }
        return null;

        // Nested helper method to improve readability
        void RemoveVisitedNeighbours()
        {
            // Remove all visited neigbours and return null if there are no unvisited neighbours remaining
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i].IsVisited)
                {
                    neighbours.RemoveAt(i);
                    i--; // accounts for the removed element and the subsequent index shift of the remaining elements
                }
            }
        }
    }
    #endregion
}
