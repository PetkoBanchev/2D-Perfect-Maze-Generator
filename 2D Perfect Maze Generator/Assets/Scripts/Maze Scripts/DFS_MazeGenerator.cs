using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS_MazeGenerator : MonoBehaviour, IMazeGenerator
{
    #region Private Variables

    private ICell currentCell;
    private Stack<ICell> stack;
    private Algorithm algorithm = Algorithm.DEPTH_FIRST_SEARCH;

    #endregion

    #region Public Properties

    public Algorithm Algorithm { get { return algorithm; } }

    #endregion

    #region Private Methods

    /// <summary>
    /// https://en.wikipedia.org/wiki/Maze_generation_algorithm
    /// 
    /// 1. Choose the initial cell, mark it as visited and push it to the stack
    /// 2. While the stack is not empty
    ///     1a. Pop a cell from the stack and make it a current cell
    ///     2a. If the current cell has any neighbours which have not been visited
    ///        1b.  Push the current cell to the stack
    ///        2b.  Choose one of the unvisited neighbours
    ///        3b.  Remove the wall between the current cell and the chosen cell
    ///        4b.  Mark the chosen cell as visited and push it to the stack
    /// </summary>
    /// <returns></returns>
    private IEnumerator DepthFirstSearchAlgorithm()
    {
        stack = new Stack<ICell>();
        var wallRemover = MazeManager.Instance.GetCurrentWallRemover();
        var isAnimated = MazeManager.Instance.IsGenerationAnimated;

        // 1
        currentCell = MazeManager.Instance.GetRandomCell();
        currentCell.IsVisited = true;
        stack.Push(currentCell);

        //2
        while (stack.Count > 0)
        {
            // 1a
            currentCell = stack.Pop();

            if (isAnimated)
                currentCell.SetColor(Color.blue);

            // 2a and 2b
            var nextCell = currentCell.GetRandomUnvisitedNeighbour();
            if (nextCell != null)
            {
                stack.Push(currentCell); // 1b
                wallRemover.RemoveWalls(currentCell, nextCell); // 3b
                nextCell.IsVisited = true; // 4b
                stack.Push(nextCell); // 4b

                if (isAnimated)
                    yield return new WaitForSeconds(0);
            }

            if (isAnimated)
                currentCell.SetColor(Color.green);
        }
        yield return null;
    }

    #endregion

    #region Public Methods

    public void GenerateMaze()
    {
        StopAllCoroutines(); // Must always stop the previous coroutine before starting a new one otherwise there's a risk of multiple coroutines running at the same time upon maze regeneration.
        StartCoroutine(DepthFirstSearchAlgorithm());
    }

    /// <summary>
    /// Remote stop of the coroutines is necesarry to keep the algorithm swap bug free.
    /// Not stoping the coroutine upon algorithm swap risks multiple coroutines running at the same time.
    /// </summary>
    public void StopMazeGeneration()
    {
        StopAllCoroutines();
    }

    #endregion

}
