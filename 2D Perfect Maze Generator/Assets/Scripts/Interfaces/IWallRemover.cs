public interface IWallRemover
{
    public Cell CellType { get; }
    public void RemoveWalls(ICell currentCell, ICell nextCell);
}
