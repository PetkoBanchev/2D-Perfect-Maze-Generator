using System.Collections;
using UnityEngine;

public class AB_MazeGenerator : MonoBehaviour, IMazeGenerator
{
    #region Private Variables

    private ICell currentCell;
    private Algorithm algorithm = Algorithm.ALDOUS_BRODER;

    #endregion

    #region Public Properties

    public Algorithm Algorithm { get { return algorithm; } }

    #endregion

    #region Private Methods

    /// <summary>
    /// https://en.wikipedia.org/wiki/Maze_generation_algorithm
    /// 
    /// 1. Pick a random cell as the current cell and mark it as visited.
    /// 2. While there are unvisited cells:
    ///     1a. Pick a random neighbour.
    ///     2a. If the chosen neighbour has not been visited:
    ///         1b. Remove the wall between the current cell and the chosen neighbour.
    ///         2b. Mark the chosen neighbour as visited.
    ///     3a. Make the chosen neighbour the current cell.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AldousBroderAlgorithm()
    {
        var wallRemover = MazeManager.Instance.GetCurrentWallRemover(); // Caching the current wall remover
        var isAnimated = MazeManager.Instance.IsGenerationAnimated;
        var unvisitedCells = MazeManager.Instance.CellCount;

        // 1
        currentCell = MazeManager.Instance.GetRandomCell();
        currentCell.IsVisited = true;
        unvisitedCells--;

        // 2
        while (unvisitedCells > 0)
        {
            var neighbour = currentCell.GetRandomNeighbour();  //1a

            if (isAnimated)
                currentCell.SetColor(Color.blue);

            // 2a
            if(!neighbour.IsVisited)
            {
                wallRemover.RemoveWalls(currentCell, neighbour); // 1b
                neighbour.IsVisited = true; // 2b
                unvisitedCells--;

                if (isAnimated)
                    yield return new WaitForSeconds(0);

            }

            if (isAnimated)
                currentCell.SetColor(Color.yellow);

            currentCell = neighbour; // 3a

        }

        if (isAnimated)
            currentCell.SetColor(Color.yellow);

        yield return null;
    }

    #endregion
   
    #region Public Methods

    public void GenerateMaze()
    {
        StopAllCoroutines(); // Must always stop the previous coroutine before starting a new one otherwise there's a risk of multiple coroutines running at the same time upon maze regeneration.
        StartCoroutine(AldousBroderAlgorithm());
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
