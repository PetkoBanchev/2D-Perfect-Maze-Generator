using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCell : MonoBehaviour, ICell
{
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private bool isVisited = false;
    [SerializeField] private GameObject[] wallObjects;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private List<ICell> unvisitedNeighbours = new List<ICell>();
    private List<ICell> neighbours = new List<ICell>();
    private bool areNeighboursCached = false;
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

    private void CacheUnvisitedNeighbours()
    {
        //Top Neighbour (x, y + 1)
        if (y + 1 < MazeManager.Instance.Height)
        {
            var neigbhour = MazeManager.Instance.GetCell(x, y + 1);
            neighbours.Add(neigbhour);
            if (!neigbhour.IsVisited)
                unvisitedNeighbours.Add(neigbhour);
        }
        //Right Neighbour (x +1, y)
        if (x + 1 < MazeManager.Instance.Width)
        {
            var neigbhour = MazeManager.Instance.GetCell(x + 1, y);
            neighbours.Add(neigbhour);
            if (!neigbhour.IsVisited)
                unvisitedNeighbours.Add(neigbhour);
        }
        //Bottom Neighbour (x, y - 1)
        if (y - 1 >= 0)
        {
            var neigbhour = MazeManager.Instance.GetCell(x, y - 1);
            neighbours.Add(neigbhour);
            if (!neigbhour.IsVisited)
                unvisitedNeighbours.Add(neigbhour);
        }
        //Left Neighbour (x - 1, y)
        if (x - 1 >= 0)
        {
            var neigbhour = MazeManager.Instance.GetCell(x - 1, y);
            neighbours.Add(neigbhour);
            if (!neigbhour.IsVisited)
                unvisitedNeighbours.Add(neigbhour);
        }
        areNeighboursCached = true;
    }
    public ICell GetRandomUnvisitedNeighbour()
    {
        if (!areNeighboursCached)
            CacheUnvisitedNeighbours();

        if (unvisitedNeighbours.Count > 0)
        {
            RemoveVisitedNeighbours();
            // Checks if any unvisited neighbours remain. If there are none it returns null
            if (unvisitedNeighbours.Count <= 0)
                return null;
            // Return a random neigbhour. It is safe to not check if the randomNeighbour has been visited, since all visited neighbours are removed above.
            int randomIndex = Random.Range(0, unvisitedNeighbours.Count);
            return unvisitedNeighbours[randomIndex];

        }
        return null;

        // Nested helper method to improve readability
        void RemoveVisitedNeighbours()
        {
            // Remove all visited neigbours and return null if there are no unvisited neighbours remaining
            for (int i = 0; i < unvisitedNeighbours.Count; i++)
            {
                if (unvisitedNeighbours[i].IsVisited)
                {
                    unvisitedNeighbours.RemoveAt(i);
                    i--; // accounts for the removed element and the subsequent index shift of the remaining elements
                }
            }
        }
    }

    public ICell GetRandomNeighbour()
    {
        if (!areNeighboursCached)
        {
            CacheUnvisitedNeighbours();
        }
        var randomIndex = Random.Range(0, neighbours.Count);
        return neighbours[randomIndex];
    }
    public void RemoveWalls(Wall wall)
    {
        switch (wall)
        {
            case Wall.TOP:
                wallObjects[0].SetActive(false);
                break;
            case Wall.RIGHT:
                wallObjects[1].SetActive(false);
                break;
            case Wall.BOTTOM:
                wallObjects[2].SetActive(false);
                break;
            case Wall.LEFT:
                wallObjects[3].SetActive(false);
                break;
        }
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
    
}
