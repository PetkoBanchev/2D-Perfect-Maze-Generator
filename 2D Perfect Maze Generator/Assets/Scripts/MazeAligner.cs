using UnityEngine;
public class MazeAligner : MonoBehaviour
{

    private void Start()
    {
        MazeManager.Instance.OnEmptyMazeSet += AlignMaze;
    }
    private void AlignMaze()
    {
        float halfWidth = CalculateHalf(MazeManager.Instance.Width);
        float halfHeight = CalculateHalf(MazeManager.Instance.Height); ;

        transform.position = new Vector3( -halfWidth, -halfHeight, 0f);
        SetCameraPosition();
        
        float CalculateHalf(float whole)
        {
            float half = (whole / 2) - 0.5f;
            return half;
        }

        void SetCameraPosition()
        {
            if (halfHeight > halfWidth)
                Camera.main.orthographicSize = halfHeight + 1;
            else
                Camera.main.orthographicSize = halfWidth + 1;
        }
    }

}
