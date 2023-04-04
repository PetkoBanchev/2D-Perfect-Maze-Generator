using System.Collections;
using UnityEngine;

public class AB_MazeGenerator : MonoBehaviour, IMazeGenerator
{
    private ICell currentCell;
    private Algorithm algorithm = Algorithm.ALDOUS_BRODER;

    public Algorithm Algorithm { get { return algorithm; } }

    public void GenerateMaze()
    {
        StartCoroutine(AldousBroderAlgorithm());
    }

    private IEnumerator AldousBroderAlgorithm()
    {

        var wallRemover = MazeManager.Instance.GetCurrentWallRemover(); // Caching the current wall remover
        var isAnimated = MazeManager.Instance.IsGenerationAnimated;
        var unvisitedCells = MazeManager.Instance.MazeHolder.childCount;

        currentCell = MazeManager.Instance.GetRandomCell();
        currentCell.IsVisited = true;

        while(unvisitedCells - 1 > 0)
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
}
