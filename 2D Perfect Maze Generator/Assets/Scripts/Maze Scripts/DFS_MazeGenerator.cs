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

    private IEnumerator DepthFirstSearchAlgorithm()
    {
        stack = new Stack<ICell>();
        var wallRemover = MazeManager.Instance.GetCurrentWallRemover();
        var isAnimated = MazeManager.Instance.IsGenerationAnimated;

        currentCell = MazeManager.Instance.GetRandomCell();
        currentCell.IsVisited = true;
        stack.Push(currentCell);

        while (stack.Count > 0)
        {

            currentCell = stack.Pop();

            if (isAnimated)
                currentCell.SetColor(Color.blue);

            var nextCell = currentCell.GetRandomUnvisitedNeighbour();
            if (nextCell != null)
            {
                stack.Push(currentCell);
                wallRemover.RemoveWalls(currentCell, nextCell);
                nextCell.IsVisited = true;
                stack.Push(nextCell);

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
