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
        this.min = min;
        this.max = max;
        map = new GameObject[mazeSize, mazeSize];
        for (int i = 0; i < mazeSize; i++)
        {
            for (int j = 0; j < mazeSize; j++)
            {
                GameObject temp = Instantiate(cubePrefab) as GameObject;
                temp.transform.parent = this.transform;
                temp.transform.position = new Vector3(i, 0, j);
                map[i, j] = temp;
                temp.GetComponent<NavMeshSurface>().BuildNavMesh();
            }
        }
    }

}
