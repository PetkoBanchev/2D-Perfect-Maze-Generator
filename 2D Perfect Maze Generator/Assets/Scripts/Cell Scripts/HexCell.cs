using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour, ICell
{
    #region Private Variables

    [SerializeField] private Vector2 coordinates;
    [SerializeField] private bool isVisited = false;
    [SerializeField] private Dictionary<Wall, GameObject> walls;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private List<ICell> unvisitedNeighbours = new List<ICell>();
    private List<ICell> neighbours = new List<ICell>();

    private bool areNeighboursCached = false;
    #endregion

    #region Public Properties

    public Vector2 Coordinates 
    { 
        get { return coordinates; }
        set { coordinates = value; }
    }

    public bool IsVisited
    {
        get { return isVisited; }
        set { isVisited = value; }
    }
    #endregion

    #region Private Methods

    private void Awake()
    {
        CacheWalls();
    }

    /// <summary>
    /// Caches the walls in a dictionary with the Wall enum as a key.
    /// </summary>
    private void CacheWalls()
    {
        walls = new Dictionary<Wall, GameObject>();
        var allWalls = GetComponentsInChildren<WallScript>();
        foreach (var wall in allWalls)
            walls.Add(wall.WallType, wall.gameObject);
    }

    /// <summary>
    /// Caches all neighbours. Keeps track of the unvisited neighbours in a separate list. 
    /// Simple if check makes sure we stay inside the bounds of the maze.
    /// </summary>
    private void CacheNeighbours()
    {
        var x = coordinates.x;
        var y = coordinates.y;

        //Top Right Neighbour (x + 1, y + 1)
        if (x + 1 < MazeManager.Instance.Width && y + 1 < MazeManager.Instance.Height)
        {
            var neigbhour = MazeManager.Instance.GetCell( new Vector2(x + 1, y + 1));
            neighbours.Add(neigbhour);
            if (!neigbhour.IsVisited)
                unvisitedNeighbours.Add(neigbhour);
        }
        //Right Neigbhour (x + 2, y)
        if (x + 2 < MazeManager.Instance.Width)
        {
            var neigbhour = MazeManager.Instance.GetCell( new Vector2(x + 2, y));
            neighbours.Add(neigbhour);
            if (!neigbhour.IsVisited)
                unvisitedNeighbours.Add(neigbhour);
        }
        //Bottom Right Neighbour (x + 1, y - 1)
        if (x + 1 < MazeManager.Instance.Width && y - 1 >= 0)
        {
            var neigbhour = MazeManager.Instance.GetCell( new Vector2(x + 1, y - 1));
            neighbours.Add(neigbhour);
            if (!neigbhour.IsVisited)
                unvisitedNeighbours.Add(neigbhour);
        }
        //Bottom Left Neighbour (x - 1, y - 1)
        if (x - 1 >= 0 && y - 1 >= 0)
        {
            var neigbhour = MazeManager.Instance.GetCell( new Vector2(x - 1, y - 1));
            neighbours.Add(neigbhour);
            if (!neigbhour.IsVisited)
                unvisitedNeighbours.Add(neigbhour);
        }
        //Left Neigbhour (x - 2, y)
        if (x - 2 >= 0)
        {
            var neigbhour = MazeManager.Instance.GetCell( new Vector2(x - 2, y));
            neighbours.Add(neigbhour);
            if (!neigbhour.IsVisited)
                unvisitedNeighbours.Add(neigbhour);
        }
        //Top Left Neighbour (x - 1, y + 1)
        if (x - 1 >= 0 && y + 1 < MazeManager.Instance.Height)
        {
            var neigbhour = MazeManager.Instance.GetCell( new Vector2(x - 1, y + 1));
            neighbours.Add(neigbhour);
            if (!neigbhour.IsVisited)
                unvisitedNeighbours.Add(neigbhour);
        }

        areNeighboursCached = true;
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Caches all neighbours on the first call.
    /// Checks if any unvisited neighbours remain. If there are non it returns null.
    /// If there are some, we first remove any visited neighbours and perform another count. If there are no unvisited neighbours remaining it returns null.
    /// Finally, a random index is generated and a unvisited neighbour is returned.
    /// </summary>
    /// <returns></returns>
    public ICell GetRandomUnvisitedNeighbour()
    {
        if (!areNeighboursCached)
            CacheNeighbours();

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

    /// <summary>
    /// Caches all neighbours on the first call.
    /// Generates a random index and returns the chosen neighbour.
    /// </summary>
    /// <returns></returns>
    public ICell GetRandomNeighbour()
    {
        if (!areNeighboursCached)
        {
            CacheNeighbours();
        }
        var randomIndex = Random.Range(0, neighbours.Count);
        return neighbours[randomIndex];
    }

    /// <summary>
    /// Removes a wall based on the given key
    /// </summary>
    /// <param name="wall"></param>
    public void RemoveWalls(Wall wall)
    {
        walls[wall].SetActive(false);   
    }

    public void SetColor(Color color)
    {
        if (spriteRenderer != null)
            spriteRenderer.material.color = color;
    }
    #endregion
}
