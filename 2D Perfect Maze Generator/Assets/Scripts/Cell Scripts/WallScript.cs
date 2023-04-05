using UnityEngine;

public class WallScript : MonoBehaviour 
{
    [SerializeField] private Wall wallType;
    public Wall WallType { get { return wallType; } }
}
