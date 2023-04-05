using UnityEngine;
public class MazeAligner : MonoBehaviour
{
    #region Private Methods
    private void Start()
    {
        MazeManager.Instance.OnEmptyMazeSet += AlignMaze;
    }

    /// <summary>
    /// Moves the MazeHolder so the maze is centred on the screen.
    /// Adjusts the camera's orthographic size so the maze fits on the screen.
    /// </summary>
    private void AlignMaze()
    {
        float halfWidth = CalculateHalf(MazeManager.Instance.Width);
        float halfHeight = CalculateHalf(MazeManager.Instance.Height); ;

        if (MazeManager.Instance.CellType == Cell.SQUARE)
            transform.position = new Vector3(-halfWidth, -halfHeight, 0f);
        else
            transform.position = new Vector3(-(MazeManager.Instance.Width/5), -(MazeManager.Instance.Height/3));

        SetCameraSize();

        #region Nested Helper Methods
        float CalculateHalf(float whole)
        {
            float half = (whole / 2) - 0.5f;
            return half;
        }

        void SetCameraSize()
        {
            if (halfHeight > halfWidth)
                Camera.main.orthographicSize = halfHeight + 1;
            else
                Camera.main.orthographicSize = halfWidth + 1;
        }

        #endregion
    }

    #endregion
}
