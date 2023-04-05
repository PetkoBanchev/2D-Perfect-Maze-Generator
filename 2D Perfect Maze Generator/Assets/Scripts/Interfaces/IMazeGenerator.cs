using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMazeGenerator
{
    public Algorithm Algorithm { get; }
    public void GenerateMaze();
    public void StopMazeGeneration();
}
