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
    private IGridGenerator gridGenerator;
    private MazeGenerator mazeGenerator;
    private Cell cellType = Cell.SQUARE;

    private ICell[,] maze;

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

    

    public Transform MazeHolder { get { return mazeHolder; } }
    #endregion

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        UIManager.Instance.OnGenerateMazePressed += CreateNewMaze;
        GetComponent<SquareGridGenerator>().OnEmptyGridGenerated += SetEmptyMaze;
        gridGenerator = GetComponent<IGridGenerator>();
        mazeGenerator = GetComponent<MazeGenerator>();  
    }
    private void CreateNewMaze()
    {
        DeleteMaze();
        gridGenerator.GenerateEmptyGrid();
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
