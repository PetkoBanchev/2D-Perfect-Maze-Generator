using System;
public interface IGridGenerator
{
    public Cell CellType { get; }
    public event Action<ICell[,]> OnEmptyGridGenerated;
    public void GenerateEmptyGrid();
}
