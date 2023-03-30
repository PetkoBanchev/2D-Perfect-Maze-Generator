using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCell : MonoBehaviour, ICell
{
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private bool isVisited = false;
    [SerializeField] private GameObject[] wallObjects;

    private List<ICell> neighbours = new List<ICell>();

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

    ICell ICell.GetRandomUnvisitedNeighbour()
    {
        throw new System.NotImplementedException();
    }

    void ICell.RemoveWalls(Wall wall)
    {
        throw new System.NotImplementedException();
    }

    void ICell.SetColor(Color color)
    {
        throw new System.NotImplementedException();
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
