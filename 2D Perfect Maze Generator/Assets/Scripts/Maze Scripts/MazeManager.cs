using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeManager : MonoBehaviour
{
    [SerializeField, Range(10, 250)] int width = 10;
    [SerializeField, Range(10, 250)] int height = 10;
    [SerializeField] private bool isGenerationAnimated;
    [SerializeField] private Transform mazeHolder;
    private MazeGenerator mazeGenerator;
    private Cell cellType = Cell.SQUARE;

    private ICell[,] maze;
    private Dictionary<Cell, IGridGenerator> gridGenerators;

    #region Singleton
    private static MazeManager instance;
    public static MazeManager Instance { get { return instance; } }
    #endregion

    public event Action OnEmptyMazeSet;

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

    public Cell CellType
    {
        get { return cellType; }
        set 
        {
            gridGenerators[cellType].OnEmptyGridGenerated -= SetEmptyMaze; // unsubscribing the old gridGenerator
            cellType = value;
            gridGenerators[cellType].OnEmptyGridGenerated += SetEmptyMaze;
        }
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
        GetAllGenerators();

        UIManager.Instance.OnGenerateMazePressed += CreateNewMaze;
        gridGenerators[cellType].OnEmptyGridGenerated += SetEmptyMaze; // default implementation
        mazeGenerator = GetComponent<MazeGenerator>();  
    }
    /// <summary>
    /// Caches all of the grid generators via relfection
    /// </summary>
    private void GetAllGenerators()
    {
        gridGenerators = new Dictionary<Cell, IGridGenerator>();
        var allGridGenerators = GetComponents<IGridGenerator>();
        foreach (var generator in allGridGenerators)
            gridGenerators.Add(generator.CellType, generator);
    }

    private void CreateNewMaze()
    {
        DeleteMaze();
        gridGenerators[cellType].GenerateEmptyGrid();
    }

    private void SetEmptyMaze(ICell[,] _maze)
    {
        maze = _maze;
        OnEmptyMazeSet?.Invoke();
        mazeGenerator.GenerateMaze();
    }

    private void DeleteMaze()
    {
        mazeHolder.position = Vector3.zero; // Must reset the position of the MazeHolder before every new maze, otherwise it will not be centred.
        foreach(Transform child in mazeHolder)
            Destroy(child.gameObject);
    }


    public ICell GetCell(int x, int y)
    {
        return maze[x, y];
    }
    public ICell GetRandomCell()
    {
        int x = 2 * Random.Range(0, width / 2);
        int y = 2 * Random.Range(0, height / 2);

        var cell = maze[x, y];
        return cell;
    }
}
