using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private ICell currentCell;
    private Stack<ICell> stack;
    public void GenerateMaze()
    {
        StartCoroutine(DepthFirstSearchAlgorithm());
    }

    private IEnumerator DepthFirstSearchAlgorithm()
    {
        stack = new Stack<ICell>();
        var wallRemover = GetComponent<IWallRemover>(); // Caching the wall remover
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
                    yield return new WaitForSeconds(10 / (MazeManager.Instance.Width * MazeManager.Instance.Height));
            }
            if (isAnimated)
                currentCell.SetColor(Color.green);
        }
        yield return null;
    }
}
