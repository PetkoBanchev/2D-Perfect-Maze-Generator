using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeManager : MonoBehaviour
{
    #region Private Variables

    [SerializeField, Range(10, 250)] int width = 10;
    [SerializeField, Range(10, 250)] int height = 10;
    [SerializeField] private bool isGenerationAnimated;
    [SerializeField] private Transform mazeHolder;

    //private ICell[,] maze;
    private Dictionary<Vector2, ICell> maze;

    [SerializeField] private Cell cellType = Cell.SQUARE; // default implementation
    [SerializeField] private Algorithm algorithm = Algorithm.DEPTH_FIRST_SEARCH; // default implementation

    private Dictionary<Cell, IGridGenerator> gridGenerators;
    private Dictionary<Cell, IWallRemover> wallRemovers;
    private Dictionary<Algorithm, IMazeGenerator> mazeGenerators;

    #endregion
    
    #region Public Properties

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

    public Cell CellType
    {
        get { return cellType; }
        set { SetCellType(value); }
    }
    public Algorithm Algorithm 
    { 
        get { return algorithm; }
        set { SetAlgorithmType(value); }
    }

    public int CellCount { get { return maze.Count; } }

    public Transform MazeHolder { get { return mazeHolder; } }

    #endregion

    #region Singleton

    private static MazeManager instance;
    public static MazeManager Instance { get { return instance; } }

    #endregion

    #region Events

    public event Action OnEmptyMazeSet;

    #endregion

    #region Private Methods

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        GetAllGridGenerators();
        GetAllWallRemovers();
        GetAllMazeGenerators();

        UIManager.Instance.OnGenerateMazePressed += CreateNewMaze; // Connects the MazeManager to the UI
        gridGenerators[cellType].OnEmptyGridGenerated += SetEmptyMaze; // default implementation
    }
    /// <summary>
    /// Caches all of the grid generators via relfection
    /// </summary>
    private void GetAllGridGenerators()
    {
        gridGenerators = new Dictionary<Cell, IGridGenerator>();
        var allGridGenerators = GetComponents<IGridGenerator>();
        foreach (var generator in allGridGenerators)
            gridGenerators.Add(generator.CellType, generator);
    }

    /// <summary>
    /// Caches all of the wall removers via relfection
    /// </summary>
    private void GetAllWallRemovers()
    {
        wallRemovers = new Dictionary<Cell, IWallRemover>();
        var allWallRemovers = GetComponents<IWallRemover>();
        foreach (var wallRemover in allWallRemovers)
            wallRemovers.Add(wallRemover.CellType, wallRemover);
    }

    /// <summary>
    /// Caches all of the maze generators via relfection
    /// </summary>
    private void GetAllMazeGenerators()
    {
        mazeGenerators = new Dictionary<Algorithm, IMazeGenerator>();
        var allMazeGenerators = GetComponents<IMazeGenerator>();
        foreach (var generator in allMazeGenerators)
            mazeGenerators.Add(generator.Algorithm, generator);
    }

    /// <summary>
    /// This is the method connected to the Generate Maze button in the UI via the OnGenerateMazePressed event.
    /// First it deletes the old maze.
    /// The starts the grid generation process in the specified gridGenerator.
    /// </summary>
    private void CreateNewMaze()
    {
        DeleteMaze();
        
        // Doubles the width only when using hexagons because the hex grid is generated using doubled coordinates.
        // When unaccounted for, doubled coordinates produces a rectangular grid that is half of the given width.
        // E.g. A width of 10 results in a 5 cell wide grid.
        // That is why we double the width.
        if (cellType == Cell.HEXAGON)
            width *= 2;
        
        gridGenerators[cellType].GenerateEmptyGrid();
    }

    /// <summary>
    /// Connected to the specified gridGenerator via the OnEmptyGridGenerated event.
    /// Caches the empty grid and invokes the OnEmptyMazeSet event.
    /// Finaly, it starts the maze generation process in the specified mazeGenerator.
    /// </summary>
    /// <param name="_maze"></param>
    private void SetEmptyMaze(Dictionary<Vector2, ICell> _maze)
    {
        maze = _maze;
        OnEmptyMazeSet?.Invoke();
        mazeGenerators[algorithm].GenerateMaze();
    }

    /// <summary>
    /// Recieves the CellType from the UI and handles the corresponding event subscription and unsubscription.
    /// </summary>
    /// <param name="_cellType"></param>
    private void SetCellType(Cell _cellType)
    {
        gridGenerators[cellType].OnEmptyGridGenerated -= SetEmptyMaze; // unsubscribing the old gridGenerator
        cellType = _cellType;
        gridGenerators[cellType].OnEmptyGridGenerated += SetEmptyMaze; // subscribing the new gridGenerator
    }

    /// <summary>
    /// Recieves the AlgorithmType from the UI and stops the coroutine in the old MazeGenerator
    /// </summary>
    /// <param name="_cellType"></param>
    private void SetAlgorithmType(Algorithm _algorithmType)
    {
        mazeGenerators[algorithm].StopMazeGeneration(); // stops the old maze generator and prevents multiple coroutines running at the same time;
        algorithm = _algorithmType;
    }

    /// <summary>
    /// Resets the position of the MazeHolder and removes all of its children.
    /// </summary>
    private void DeleteMaze()
    {
        mazeHolder.position = Vector3.zero; // Must reset the position of the MazeHolder before every new maze, otherwise it will not be centred.
        foreach(Transform child in mazeHolder)
            Destroy(child.gameObject);
    }

    #endregion

    #region Public Methods

    public ICell GetCell(Vector2 coordinates)
    {
        return maze[coordinates];
    }
    public ICell GetRandomCell()
    {
        int x = 2 * Random.Range(0, width / 2);
        int y = 2 * Random.Range(0, height / 2);

        var cell = maze[new Vector2(x, y)];
        return cell;
    }

    public IWallRemover GetCurrentWallRemover() 
    {
        return wallRemovers[cellType];
    }

    #endregion
}
