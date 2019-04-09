using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    int mazeSize = 30;
    public GameObject cubePrefab;
    GameObject[,] map;
    int min, max;

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
                temp.transform.position = new Vector3(i, 0, j);
                map[i, j] = temp;
            }
        }
    }

}
