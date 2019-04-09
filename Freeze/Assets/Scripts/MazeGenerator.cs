using UnityEngine;
using UnityEngine.AI;
public class MazeGenerator : MonoBehaviour
{
    int mazeSize = 30;
    public GameObject cubePrefab;
    GameObject[,] map;
    int min, max;

    private void Start()
    {
        Generate(mazeSize, 10, 30);
    }
    public void Generate(int spawners, int min, int max)
    {

    }

}
