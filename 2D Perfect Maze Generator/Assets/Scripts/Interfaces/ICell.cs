using UnityEngine;

public interface ICell
{
    //public int X { get; set; }
    //public int Y { get; set; }
    public Vector2 Coordinates { get; set; }
    public bool IsVisited { get; set; }

    public void RemoveWalls(Wall wall);
    public ICell GetRandomNeighbour();
    public ICell GetRandomUnvisitedNeighbour();
    public void SetColor(Color color);
}
