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

    private IEnumerator AldousBroderAlgorithm()
    {
        var wallRemover = MazeManager.Instance.GetCurrentWallRemover(); // Caching the current wall remover
        var isAnimated = MazeManager.Instance.IsGenerationAnimated;
        var unvisitedCells = MazeManager.Instance.CellCount;

        currentCell = MazeManager.Instance.GetRandomCell();
        currentCell.IsVisited = true;
        unvisitedCells--;

        while (unvisitedCells > 0)
        {
            var neighbour = currentCell.GetRandomNeighbour();

            if (isAnimated)
                currentCell.SetColor(Color.blue);

            if(!neighbour.IsVisited)
            {
                wallRemover.RemoveWalls(currentCell, neighbour);
                neighbour.IsVisited = true;
                unvisitedCells--;

                if (isAnimated)
                    yield return new WaitForSeconds(0);

            }

            if (isAnimated)
                currentCell.SetColor(Color.yellow);

            currentCell = neighbour;

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
