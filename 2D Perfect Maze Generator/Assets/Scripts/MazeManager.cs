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
    private Cell cellType = Cell.SQUARE;

    private ICell[,] maze;
    private ICell currentCell;
    private Stack<ICell> stack;

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
        stack = new Stack<ICell>();
        GetComponent<SquareGridGenerator>().OnEmptyGridGenerated += SetMaze;
        GetComponent<SquareGridGenerator>().GenerateEmptyGrid();
        StartCoroutine(DepthFirstSearchAlgorithm());
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SetMaze(ICell[,] _maze)
    {
        maze = _maze;
        Debug.Log("maze set");
    }

    private IEnumerator DepthFirstSearchAlgorithm()
    {
        yield return new WaitForSeconds(1);//small delay to prevent the coroutine from starting before the maze is set. It will be fixed
        var wallRemover = GetComponent<IWallRemover>(); // Caching the wall remover
        currentCell = GetRandomCell();
        currentCell.IsVisited = true;
        stack.Push(currentCell);
        while (stack.Count > 0)
        {
            currentCell = stack.Pop();
            if (isGenerationAnimated)
                currentCell.SetColor(Color.blue);
            var nextCell = currentCell.GetRandomUnvisitedNeighbour();
            if (nextCell != null)
            {
                stack.Push(currentCell);
                wallRemover.RemoveWalls(currentCell, nextCell);
                nextCell.IsVisited = true;
                stack.Push(nextCell);
                if (isGenerationAnimated)
                    yield return new WaitForSeconds(10 / (width * height));
            }
            if (isGenerationAnimated)
                currentCell.SetColor(Color.green);
        }
        yield return null;
    }

    private ICell GetRandomCell()
    {
        int x = 2 * Random.Range(0, width / 2);
        int y = 2 * Random.Range(0, height / 2);

        var cell = maze[x, y];
        return cell;
    }
}
