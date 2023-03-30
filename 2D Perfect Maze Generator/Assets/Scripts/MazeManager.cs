using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField, Range(10, 250)] int width = 10;
    [SerializeField, Range(10, 250)] int height = 10;
    [SerializeField] private bool isGenerationAnimated;
    [SerializeField] private Transform mazeHolder;
    private Cell cellType = Cell.SQUARE;

    private ICell[,] maze;

    #region Singleton
    private static MazeManager instance;
    public static MazeManager Instance { get { return instance; } }
    #endregion

    #region Public properties
    public int Width
    {
        get { return width; }
        set { width = value; }
    }
    public int Height
    {
        get { return height; }
        set { height = value; }
    }

    public bool IsGenerationAnimated
    {
        get { return isGenerationAnimated; }
        set { isGenerationAnimated = value; }
    }

    public ICell GetCell(int x, int y)
    {
        return maze[x, y];
    }

    public Transform MazeHolder { get { return mazeHolder; } }
    #endregion

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

    }
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<SquareGridGenerator>().OnEmptyGridGenerated += SetMaze;
        GetComponent<SquareGridGenerator>().GenerateEmptyGrid();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SetMaze(ICell[,] _maze)
    {
        maze = _maze;
    }
}
