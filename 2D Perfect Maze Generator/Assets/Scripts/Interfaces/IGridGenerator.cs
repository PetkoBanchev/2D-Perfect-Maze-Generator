using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGridGenerator
{
    public Cell CellType { get; }
    public event Action<Dictionary<Vector2, ICell>> OnEmptyGridGenerated;
    public void GenerateEmptyGrid();
}
