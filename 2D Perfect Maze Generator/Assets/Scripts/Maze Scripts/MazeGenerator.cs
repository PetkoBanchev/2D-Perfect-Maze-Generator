using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private ICell currentCell;
    private Stack<ICell> stack;
    private Dictionary<Cell, IWallRemover> wallRemovers;

    private void Awake()
    {
        GetAllWallRemovers();
    }
    public void GenerateMaze()
    {
        StartCoroutine(DepthFirstSearchAlgorithm());
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
    private IEnumerator DepthFirstSearchAlgorithm()
    {
        stack = new Stack<ICell>();
        var wallRemover = wallRemovers[MazeManager.Instance.CellType]; // Caching the current wall remover
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
}
